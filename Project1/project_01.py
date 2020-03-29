import cv2
import numpy as np
from math import hypot

# Functions -------------------------------------------------------------------------

def imageResize(image, width = None, height = None, inter = cv2.INTER_AREA):
    (h, w) = image.shape[:2]
    r = width / float(w)
    dim = (width, int(h * r))
    result = cv2.resize(image, dim, inter)

    return result

# Source is what we want to find and tarjet is the object where we want to find the source
def templateMatch(target, source, threshold, scaleFactor = 1.0):
    target_height, target_width, _ = target.shape
    source_height, source_width, _ = source.shape

    result_height = source_height - target_height + 1
    result_width = source_width - target_width + 1

    indexes = [] # will contain an array of the i and j when a match is found

    mm_h, mm_w, _ = source.shape
    matchingMap = np.zeros((mm_h, mm_w))
    ssd = 0

    for i in range(0, result_height):
        for j in range(0, result_width):
            diff = target[0][0] - source[i][j]
            ssd += diff * diff
            matchingMap[i][j] = hypot(ssd[0], ssd[1], ssd[2])
            if CompareSSD(ssd, threshold):
                for i_1 in range (0, target_height):
                    for j_1 in range (0, target_width):
                        n_diff = target[i_1][j_1] - source[i+i_1][j+j_1]
                        ssd += n_diff * n_diff
            if CompareSSD(ssd, threshold): 
                print("Match found, ssd: " + str(ssd)) 
                indexes.append([i, j])
            ssd = 0
     
    # for down-scaled images, we need to upscale the found indexes and the matching map
    if(scaleFactor != 1.0): 
        for i in range(len(indexes)): 
            index_i = indexes[i]
            index_i[0] =  (int)(index_i[0] / scaleFactor)
            index_i[1] =  (int)(index_i[1] / scaleFactor)
        matchingMap = imageResize(matchingMap, (int)(source_width / scaleFactor), (int)(source_height / scaleFactor))
    
    return [indexes, matchingMap]

# -----------------------------------------------------------------------------------

def CompareSSD(ssd, magnitude): 
    if(hypot(ssd[0],ssd[1],ssd[2]) <= magnitude): 
        return True
    else: 
        return False

def fillMatch(img, index_i, index_j, width, height): 
    return cv2.rectangle(img, (index_j, index_i), (index_j + height, index_i + width), (0, 255, 0), thickness=2)  

scaleFactorUsed = 0.5

target_image = cv2.imread("images/t1-img1.png")
height_t, width_t, _ = target_image.shape
target_image_resized = imageResize(target_image, (int)(width_t * scaleFactorUsed), (int)(height_t * scaleFactorUsed)) 
target_image_resized = target_image_resized / 255

source_image = cv2.imread("images/img1.png")
height_s, width_s, _ = source_image.shape 
source_image_resized = imageResize(source_image, (int)(width_s * scaleFactorUsed), (int)(height_s * scaleFactorUsed))
source_image_resized = source_image_resized / 255

thresholdo = 2 # use 0.1 threshold if scaleFactorUsed is 1.0
indexes_result, matching_map = templateMatch(target_image_resized, source_image_resized, thresholdo, scaleFactorUsed)
source_height_result, source_width_result, _ = source_image.shape

rects = []
for i_i in range(len(indexes_result)): 
    index = indexes_result[i_i]
    rects.append(fillMatch(source_image, index[0], index[1], height_t, width_t))

cv2.imshow("Source Image", source_image)

cv2.imshow("Target Image", target_image)

cv2.imshow("Matching Map", matching_map)

text_image = np.zeros((40, 340, 3), np.uint8)

if(len(rects) >= 1): 
    font = cv2.FONT_HERSHEY_SIMPLEX
    cv2.putText(text_image, "Target found", (5, 30), font, 1, (0, 255, 255), 2)
else:
    font = cv2.FONT_HERSHEY_SIMPLEX
    cv2.putText(text_image, "Target not found", (5, 30), font, 1, (0, 0, 255), 2)

cv2.imshow("Result", text_image)

# Input ---------
key = cv2.waitKey(0)
if key == 27:
    cv2.destroyAllWindows()

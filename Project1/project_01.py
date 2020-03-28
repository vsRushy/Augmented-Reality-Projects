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
    matchingMap = np.zeros(source.shape)
    ssd = 0

    for i in range(0, result_height):
        for j in range(0, result_width):
            diff = target[0][0] - source[i][j]
            ssd += diff * diff
            matchingMap[i][j] = ssd
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

target_image = cv2.imread("images/img1.png")
height_t, width_t, _ = target_image.shape
target_image_resized = imageResize(target_image, (int)(width_t * scaleFactorUsed), (int)(height_t * scaleFactorUsed)) 
target_image_resized = target_image_resized / 255

source_image = cv2.imread("images/t1-img1.png")
height_s, width_s, _ = source_image.shape 
source_image_resized = imageResize(source_image, (int)(width_s * scaleFactorUsed), (int)(height_s * scaleFactorUsed))
source_image_resized = source_image_resized / 255

cv2.imshow("Source Image", source_image)

thresholdo = 2 # use 0.1 threshold if scaleFactorUsed is 1.0
indexes_result, matching_map = templateMatch(source_image_resized, target_image_resized, thresholdo, scaleFactorUsed)
source_height_result, source_width_result, _ = source_image.shape

rects = []
for i_i in range(len(indexes_result)): 
    index = indexes_result[i_i]
    rects.append(fillMatch(target_image, index[0], index[1], source_height_result, source_width_result))

cv2.imshow("Target Image", target_image)

cv2.imshow("Matching Map", matching_map)

foundText = "Image Not Found"
if(len(rects) >= 1): 
    foundText = "Image Found"


text_image = np.zeros((50, 1050, 3), np.uint8)
font = cv2.FONT_HERSHEY_SIMPLEX
bottom_left_corner_text = (10, 35)
font_scale = 1
font_color = (255, 255, 255)
line_type = 2
cv2.putText(
    text_image,
    foundText,
    bottom_left_corner_text,
    font,
    font_scale,
    font_color,
    line_type
    )
cv2.imshow("Result", text_image)

# Input ---------
key = cv2.waitKey(0)
if key == 27:
    cv2.destroyAllWindows()

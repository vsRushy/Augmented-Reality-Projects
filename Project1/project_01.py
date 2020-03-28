import cv2
import numpy as np
from math import hypot

# Functions -------------------------------------------------------------------------

# Source is what we want to find and tarjet is the object where we want to find the source
def templateMatch(target, source, scaleFactor = 1.0):
    target_height, target_width, _ = target.shape
    source_height, source_width, _ = source.shape

    result_height = source_height - target_height + 1
    result_width = source_width - target_width + 1

    indexes = [] # will contain an array of the i and j when a match is found
    ssd = 0

    for i in range(0, result_height):
        for j in range(0, result_width):
            diff = target[0][0] - source[i][j]
            ssd += diff * diff
            if CompareSSD(ssd, 0.1):
                for i_1 in range (0, target_height):
                    for j_1 in range (0, target_width):
                        n_diff = target[i_1][j_1] - source[i+i_1][j+j_1]
                        ssd += n_diff * n_diff
            if CompareSSD(ssd, 0.1): 
                print("Match found!")
                indexes.append([i, j])
            ssd = 0
     
    if(scaleFactor != 1.0): 
        for i in range(len(indexes)): 
            index_i = indexes[i]
            index_i /= scaleFactor
    
    return indexes

# -----------------------------------------------------------------------------------

def CompareSSD(ssd, magnitude): 
    if(hypot(ssd[0],ssd[1],ssd[2]) <= magnitude): 
        return True
    else: 
        return False

def fillMatch(img, index_i, index_j, width, height): 
    return cv2.rectangle(img, (index_j, index_i), (index_j + height, index_i + width), (0, 255, 0), thickness=2)  

scaleFactorUsed = 1
target_image = cv2.imread("images/img1.png")
target_image = target_image / 255
target_image_resized = cv2.resize(target_image, (0,0), fx=scaleFactorUsed, fy=scaleFactorUsed) 

source_image = cv2.imread("images/t1-img1.png")
source_image = source_image / 255
source_image_resized = cv2.resize(source_image, (0,0), fx=scaleFactorUsed, fy=scaleFactorUsed) 
cv2.imshow("Source Image", source_image)

threshold = 0.1
indexes_result = templateMatch(source_image_resized, target_image_resized, scaleFactorUsed)

source_height_result, source_width_result, _ = source_image.shape

rects = []
for i_i in range(len(indexes_result)): 
    index = indexes_result[i_i]
    rects.append(fillMatch(target_image, index[0], index[1], source_height_result, source_width_result))

cv2.imshow("Target Image", target_image)


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

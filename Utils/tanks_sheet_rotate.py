# Extract sprite coordinates from rotated
# Kenney spritesheet
import xml.etree.ElementTree as ET

datafile = 'E:/LearnGodot/TopDownTanks/Utils/obstacles.xml'
enum_name = 'Items'
sheet_width = 782
sheet_height = 782

# parse Kenney XML file
tree = ET.parse(datafile)
rects = {}
for node in tree.iter():
    if node.attrib.get('name'):
        name = node.attrib.get('name').replace('.png', '')
        rects[name] = []
        rects[name].append(int(node.attrib.get('x')))
        rects[name].append(int(node.attrib.get('y')))
        rects[name].append(int(node.attrib.get('width')))
        rects[name].append(int(node.attrib.get('height')))
print()
enum = 'enum Items \n{\n'
for name in rects:
    enum += '\t'+ name + ', \n'
enum += '}\n'
print(enum)
print()
#print("var regions = {")
print("Dictionary regions;")
print()
print("regions = new Dictionary();")

for name, rect in rects.items():
    x, y, w, h = rect
    # offset center
    x -= sheet_width//2
    y -= sheet_height//2
    # rotate 90deg counter-clockwise
    # and use new top-left corner
    x, y = y, -x
    w, h = h, w
    y -= h
    # remove offset
    x += sheet_width//2
    y += sheet_height//2
    #print("\t%s.%s: Rect2(%s, %s, %s, %s)," % (enum_name, name, x, y, w, h))
    print("regions.Add((int)%s.%s, new Rect2(%s,%s,%s,%s));" % (enum_name, name, x, y, w, h))
print()

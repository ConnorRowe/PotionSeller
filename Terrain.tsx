<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.1" name="Terrain" tilewidth="16" tileheight="16" tilecount="21" columns="7">
 <image source="terrain_sheet.png" width="112" height="48"/>
 <tile id="20">
  <objectgroup draworder="index" id="5">
   <object id="6" name="occluder" type="occluder" x="0" y="0">
    <polygon points="0,0 16,0 16,16 0,16"/>
   </object>
   <object id="8" name="area" type="area" x="0" y="0">
    <polygon points="0,0 16,0 16,16 0,16"/>
   </object>
  </objectgroup>
 </tile>
</tileset>

<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.1" name="SeparateTileset" tilewidth="16" tileheight="16" tilecount="21" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="0">
  <properties>
   <property name="material" value="grass"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_1.png"/>
 </tile>
 <tile id="1">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_2.png"/>
 </tile>
 <tile id="2">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_3.png"/>
 </tile>
 <tile id="3">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_4.png"/>
 </tile>
 <tile id="4">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_5.png"/>
 </tile>
 <tile id="5">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_6.png"/>
 </tile>
 <tile id="6">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_7.png"/>
 </tile>
 <tile id="7">
  <properties>
   <property name="material" value="grass"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_8.png"/>
 </tile>
 <tile id="8">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_9.png"/>
 </tile>
 <tile id="9">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_10.png"/>
 </tile>
 <tile id="10">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_11.png"/>
 </tile>
 <tile id="11">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_12.png"/>
 </tile>
 <tile id="12">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_13.png"/>
 </tile>
 <tile id="13">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_14.png"/>
 </tile>
 <tile id="14">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_15.png"/>
 </tile>
 <tile id="15">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_16.png"/>
 </tile>
 <tile id="16">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_17.png"/>
 </tile>
 <tile id="17">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_18.png"/>
 </tile>
 <tile id="18">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_19.png"/>
 </tile>
 <tile id="19">
  <properties>
   <property name="material" value="dirt"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_20.png"/>
 </tile>
 <tile id="20">
  <properties>
   <property name="material" value="stone"/>
  </properties>
  <image width="16" height="16" source="textures/terrain/terrain_21.png"/>
  <objectgroup draworder="index" id="3">
   <object id="2" name="occluder" type="occluder" x="0" y="0">
    <polygon points="0,0 16,0 16,16 0,16"/>
   </object>
   <object id="3" name="area" type="area" x="0" y="0">
    <polygon points="0,0 16,0 16,16 0,16"/>
   </object>
  </objectgroup>
 </tile>
</tileset>

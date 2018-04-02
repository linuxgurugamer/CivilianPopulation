Civilian Population
===================

![Civilian Population](https://github.com/rleroy/CivilianPopulation/blob/master/banner.png "Civilian Population")

Turn your Kerbal society into a Type 1 Civilization by adding civilian exploration! 
Your agency as research goes on gains the ability to have civilians take part on your exploration.

Once build, your ships, orbital or ground station will welcome civilian that will pay a rent to your agency, breed, grow their kids, get aged and die.
You will also be able to train and recruit civilian to become regular kerbonauts.

As this mod is still under development, all the functionnalities are not available yet.

Credits
-------

This mod is a fork from "Newbier Newb's Revamp of Civilian Population" mod.

It exists thanks to the collective work of :
- [@Tralfagar](http://forum.kerbalspaceprogram.com/index.php?/profile/150801-tralfagar/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/143823-120-newbier-newbs-revamp-of-civilian-population/).
- [@michaelhester07](http://forum.kerbalspaceprogram.com/index.php?/profile/96470-michaelhester07/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/101058-10x-civilian-population-14/).
- [@rabidninjawombat](http://forum.kerbalspaceprogram.com/index.php?/profile/108889-rabidninjawombat/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/111815-104civilian-population-1751-update-to-105-in-progress/).
- [@GGumby](http://forum.kerbalspaceprogram.com/index.php?/profile/122189-ggumby/) that can be found [here](http://forum.kerbalspaceprogram.com/index.php?/topic/140127-112ckan-civilian-populations-revived/).

Licenced under [CC BY-NC 4.0](https://creativecommons.org/licenses/by-nc/4.0/).

This mod requires [Community Resource Pack](http://forum.kerbalspaceprogram.com/index.php?/topic/83007-13-community-resource-pack/).

Any kind of help is more than welcome, pick a `TODO` above, do it, and I will be glad to accept your pull request.
If you need help to understand how to build the mod, just ask !

Electrical parts
----------------

- Netherdyne Reactor Unit DX-110
- Netherdyne Reactor Unit MX-99

Convert Enriched Uranium into Depleted Uranium, Xenon Gas and Electricity !

TODO : 
- Rebalance cost.
- Rebalance conversion speed.
- Big reactor animation won't stop.
- Turn the lights off when the reactors are off.

```
    [WRN 00:08:25.564] [ShipConstruct for CivPopReactor]: part cost (15000.0) is less than the cost of its resources (432500.0)
    [WRN 00:08:25.598] [ShipConstruct for SmallCivPopReactor]: part cost (8000.0) is less than the cost of its resources (173000.0)
```

Science parts
-------------

- Netherdyne University.

A command and research module.

TODO :
- Fix EVA problems when there is not ladder.
- Implements training module that will allow to turn civilian into regular kerbonauts.

Utility parts : Apartments
--------------------------

- Civilian Large House mk2
- Civilian Small Apartment Complex
- Civilian Large Apartment Complex
- Civilian Contractor Dock Mk1
- Civilian Small House

Living quarters for civilians.

TODO : 
- Add behaviour to the Contractor Dock to have civilian pop grow from it (Modules CivilianDockGrowth & KerbalRecruitment).
- Cannot go back in Small House.
- Add ladder to Small House.

```
    [LOG 08:21:59.524] Load(Model): CivilianPopulation/Models/Utility/surfaceAttachHouseSmall
    [ERR 08:21:59.599] Triggers on concave MeshColliders are not supported

    [ERR 08:21:59.600] Triggers on concave MeshColliders are not supported
```

Utility parts : Farms
---------------------

- Hydroponic Garden Biosphere
- Small Hydroponic Garden Biosphere
- Netherdyne Farm Biodome Mk2
- Small Hydroponic Garden Module

Use plants to grow food, purify water and air. 

TODO :
- Remove biodome top attach point.
- Animation on "Small Hydroponic Garden Module" does not work.

Utility parts : Tanks
---------------------

- Small Fertilizer Tank
- Small Waste Water Tank
- Small Sustenance Tank

TODO :
- Calibrate those to a capacity of 1 day / kerbal.

Utility parts : Drill
---------------------

- Netherdyne XL-9000 Mega Laser Drill
- Netherdyne Laser Drill

Laser drilling for surface exploitation.

Ground parts
------------

- Cruiser Landing Gear
- Cruiser Landing Gear Mk2

TODO ;
- Fix legs deployments

@see http://forum.kerbalspaceprogram.com/index.php?/topic/135250-landing-legs-in-11/&page=1

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Parts/Wheels/LandingLeg.cfg

Structural Parts
----------------

- truss18x18NoCore
- truss6x18
- truss6x6Core
- truss6x6CoreL
- truss6x6CoreT
- truss6x6CoreX
- truss6x6NoCore
- bioSphereBaseNoWalls

All seems ok.

- bioSphereWindows
- bioSphereWindowsLarge
- bioSphereWindowsWide

TODO : Cannot go inside, should improve attach points.

- bioDomeBase
- bioDomeBaseLarge
- bioSphereBase
- bioSphereBaseWallRing
- parkbioDomeBase
- parkbioDomeBaseRock
- parkbioDomeBaseMetal

TODO : Attachment point in the air, cannot go "into" the structure.

Agency
------

TODO : Add Netherdyne as an agency.

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Agencies
@see KerbalPlanetaryBaseSystems - Sources/PlanetarySurfaceStructures/SurfaceStructuresCategoryFilter.cs

Configuration for third party addons
------------------------------------

TODO : Kerbalism - Configure farms.

Civilian behaviour
------------------

DONE :

- Rent of civilian : 200 funds per civilian per day (6 hours).
- Civilian contractor growth : Every 85 days on Kerbin orbit, 170 around Mun or Minmus, never elsewhere, a civilian will be created on an activated Civilian Dock.
- Civilian recruitment process : The button recruit of the university will turn civilian living in this university into regular kerbals with a random choosen profession.
- Civilian breeding growth : If allowed (From a Habitat part or the contractors dock), Kerbals will breed. Females give birth after 320 (3/4 of a Kerbin Year) days of pregnancy. Males can breed from 15yo females from 15yo to 45yo.
- Civilian aging and death : Once a Kerbal reach 75yo, there (age-75)% chance per year that he dies from his old age any day during this year.

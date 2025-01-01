An old but good mod, this mod has had a checkered past.  It exists thanks to the collective work of :

@michaelhester07 https://forum.kerbalspaceprogram.com/index.php?/topic/101058-10x-civilian-population-14/
@rabidninjawombat https://forum.kerbalspaceprogram.com/index.php?/topic/111815-104civilian-population-1751-update-to-105-in-progress/
@GGumby https://forum.kerbalspaceprogram.com/index.php?/topic/140127-112ckan-civilian-populations-revived/
@trafalgar https://forum.kerbalspaceprogram.com/index.php?/topic/143823-120-newbier-newbs-revamp-of-civilian-population/
@Pamynx https://forum.kerbalspaceprogram.com/index.php?/topic/162204-17x-civilian-population-released-07042018/
and finally from @pmborg an unofficial rebuild here: https://forum.kerbalspaceprogram.com/index.php?/topic/162204-17x-civilian-population-released-07042018/&do=findComment&comment=3732599

 

 

https://i.imgur.com/jrwpfKa.png

 

 

 

Turn your Kerbal society into a Type 1 Civilization by adding civilian exploration! 
Your agency, as research goes on, gains the ability to have civilians take part on your exploration.

Once built, your ships, orbital or ground station will welcome civilian that will pay a rent to your agency, breed, grow their kids, get aged and die.
You will also be able to train and recruit civilian to become regular kerbonauts.

Help Needed

Most of the parts do NOT have tags.  If anyone would like to add tags, either to one, some or all parts, I'd appreciate it
 

https://i.gyazo.com/e6db5e1834a7816507b60f8e8a4b3f24.png

https://i.imgur.com/F9Xbfvs.png



 

Dependencies

Click Through Blocker
ToolbarController
Community Resource Pack
ModuleAnimateGenericEffects
Recommended Mods

Ship Manifest 
Extra Planetary Launchpads(for building huge cities on a planet)
Scansat (for resources)
Hangar Extender: For building Generation ships with the new parts in 1.2
Availability

Source: https://github.com/linuxgurugamer/CivilianPopulation
Download: https://spacedock.info/mod/2560/CivilianPopulationModernized?ga=<Game+3102+'Kerbal+Space+Program'>
License: CC-BY-NC-4.0
CKAN soon

Electrical parts
- Netherdyne Reactor Unit DX-110
- Netherdyne Reactor Unit MX-99

Convert Enriched Uranium into Depleted Uranium, Xenon Gas and Electricity !

Part List:

Civilian quarters (3 sizes)
Civilian contractor dock: allows civilian population to grow while in kerbin space
Garden modules and farm biodome to feed the civilian population
Resource containers
Laser drill for in-situ resources
Recruitment centers : Movie theater, university, flight school
Changes made during adoption

Added bulkheadProfiles to all parts
Fixed bad chars in some parts preventing them from loading
Reorganized directories
Created overall solution file
Added Assemblyversion.tt to all dlls
Added InstallChecker to all dlls
Adjusted node positions on many parts
Replaced stock toolbar code with ToolbarController
Added support for the ClickThroughBlocker
Added unique numbers for the window ids
How to manage your civilian population

Civilian kerbals are not like your astronaut kerbals. They require food to survive and have not been trained on how to eat a diet of snacks. The management of your civilian population relies on generating enough food for them. If you have enough food they'll live. If you don't they'll die off. It's that simple.

Space Tourism

Civilians will flock to your space stations and bases which can support them. You can either transport them yourself or you can hire Civilian space agencies to transport them for you. The space agencies gave Netherdyne plans for a universal docking port which their transports will use. Attach the port to your station or base and you civilians will come. Space tourism is limited to the kerbin system (Kerbin, Mun, Minmus).

Civilian reproduction and long voyages

A couple of male+female in the apartment is enough to generate a civilian. This will produce a new recruitable Kerbal every 3 kerbin months.

Understanding the stats on the Civilian Quarters parts

Every civilian quarters part will show you the statistics for your population's voyage

Food per pop: This is the minimum amount of food your ship's stores must have on hand to support the population. To get the required amount multiply this by the size of your civilian population. It's easy: 1:1. So 50 population requires a minimum of 50 food to support. Each farm module is scaled to support a 1:1 quarters to farm size. The large biospheres support the largest of population capacities. Each farm tells you how much population it can support.
Population Growth Rate: this is the time in seconds it takes for your population to grow through reproduction.
Population Decay Rate: this is the time in seconds it takes for your population to decay.
Growth and Decay timers: these show the current state of growth or decay. When these pass their respective rate the population will change by 1.
Reproduction rate: this is the minimum population your ship needs to start reproduction. The speed your population grows at scales based on how big it is beyond this. A population of 150 will reproduce 3 times faster than a population of 50.
Total consumption rate: this is how fast your population is consuming food
civilianDock: If you have a civilian contractor dock attached to your ship this indicates it.
civilianDockGrowthRate: this scales your ship's growth rate by the number indicated. Scale is 1000 for kerbin and kerbin orbit, 100 for Mun, 10 for Minmus.
GrowthRate: This is how fast your population is growing.
Recruit Kerbal: You can recruit a new crew member from the population onboard the module. Each Civilian module except the dock supports a number of Crew slots.
 

Population changes over time, even when you're not watching!

The civilian population on your ship or base will change over time even when you're doing something else (flying another mission perhaps). How the population fares depends on how well you managed the food on your ship. Make sure your ship, station, or base has food and all the farms are operational before you leave it!

Individual Parts

  Reveal hidden contents
 
=======================
Civilian Contractor Dock

https://imgur.com/MJ8p3AT.png

The Civilian Contractor dock allows civilian space agencies to deliver passengers to your station or base. You must have enough food to receive the passengers before they arrive.

Small Civilian Apartment

https://imgur.com/NrcUaTc.png

This gives a small but adequate place to stay for 8 civilians. Has 3 slots for crew. This fits a 2.5 meter diameter ship.

Large Civilian Quarters

https://imgur.com/ZazyODV.png

Has enough room for 25 civilians and 4 crew. The core is 3 meters wide.

Civilian Apartment Complex

https://imgur.com/0GU8BEa.png

Has enough room for 60 civilians and 8 crew. The core is 3 meters wide.

Farm Biodome

https://imgur.com/gSAXepj.png

Intended for use on a surface the farm biodome supports up to 60 civilians.

Garden Biosphere

https://imgur.com/Q64kxiO.png

The garden biosphere supports 50 civilians and is better suited for orbit and flight than the ground. The white core is 3 meters diameter.

Small Garden Biosphere

https://imgur.com/xwyJkU2.png

The small garden biosphere supports 25 civilians. The white core is 3 meters diameter.

Small Hydroponic Garden

https://imgur.com/xeGlATe.png

The small Hydroponic garden supports up to 8 civilians. This module fits a 2.5 meter diameter ship

Netherdyne Construction Drone

https://imgur.com/9Yp43Z1.png

The netherdyne construction drone allows users of Extraplanetary Launchpads to build bases and stations with "robots" before sending a crew up. This would make sense in a way: in real life we would probably build a moon base with robots before the first crew arrive. This part does nothing if you don't have Extraplanetary Launchpads

University

https://imgur.com/gPtqYCz.png

The orbital university has a science lab built in for processing research experiments. Can recruit Level 3 engineers and scientists

Flight school

https://imgur.com/qm1PoQ0.png

The flight school has attach points on the landing pads (though one of them has the door for the crew). Can recruit Level 3 pilots.

Movie Theater

https://imgur.com/gzavwPF.png

Kerbals must be inspired to work on a specific job. Select a type of movie to play to gain a discount on recruiting that type of crew member.

Resource pods

https://imgur.com/zrrQbih.png

Left to right:

Fertilizer container (biomass, water)

Gas container(oxygen, carbon dioxide)

Sustenance container (food, water)

Waste container (waste, wastewater)

These pods will store various resources your ship or base needs. All of them are surface attach. Most are less than 1 ton weight. The fertilizer one can get up to 7 tons though (that provides a lot of food).

inline substrate/water pod

https://imgur.com/RIIDiQU.png

This pod carries 1000 units of water and substrate mined from a surface.

Laser Drill (the green lasers on the right)

https://imgur.com/zSfhiuC.png

The laser drill allows you to extract the substrate and water from a planetary surface for your colony. It contains the converter to change the materials into biomass. This converter uses water to convert the substrate to biomass. Turns out bacteria like that material.

Managing your Civilian Population

Basics

Civilian Population requires food, water, and oxygen to survive. They will produce Carbon Dioxide, Waste Water, and Waste. Each civilian module (except the contractor dock) has space for 1 kerbin-day's worth of waste and 1 kerbin day-s worth of needs for the population that it supports.

Production of food requires biomass and water. Food is produced at a garden module or at the farm biodome.

Garden modules and the farm biodome are equipped with recyclers which can convert the waste water and waste back into water and biomass. The plants inside also act as natural scrubbers, clearing out the CO2 and producing Oxygen. The farms do not come with biomass storage. These must be attached through way of the fertilizer pod. Use of the other resource pods is recommended for an extra buffer of resources. If a waste resource builds up to the cap the excess will be dumped out into space. It is thus imperative that you keep the recycler running. The recyclers will outpace the waste production for their intended population size.

The biodomes and civilian modules are paired together to make managing this simple. The civilian population will grow once it is larger than 50 members. This growth requires a kerbal's mass in food (about 380 food). They may also come to your base or station through way of the civilian contractor dock. When they immigrate to your station, ship, or base this way they don't consume 380 food per growth. Farm food stores can be quite massive, equating to roughly 120 days worth of food for one kerbal on the small garden. That is because the colony's population growth will need the food.

https://imgur.com/zSfhiuC.png

The pictured land crawler shows everything you need to maintain your civilian population and to grow it. Left to right: mining container, garden, fertilizer for garden, Civilian pod, laser drill.

In-situ resource utilization

Water and substrate must be present in some amount at the landing site for your colony (much like MKS) for you to get the resources to grow the civilian population. The laser drill can be used to grab these resources. If you have MKS then you can use those parts to get the resources as well. The laser drill has the same efficiency as those parts.

Recruiting specific job kerbals

If you wish to recruit kerbals with specific jobs (pilot, engineer, scientist) you can use the movie theater, university, and flight school.

The movie theater is the basic part required for this. Your kerbals need inspiration for them to decide that they want to do a specific job. To get that inspiration you play movies for them in the theater. Select a type of movie to get a bonus to recruitment for the job it matches:

- Racing movies: Engineer kerbals are inspired by the act of tuning up a vehicle to win races. Playing these movies makes engineer recruitment 10% cheaper.

- Scifi movies: Pilot kerbals are inspired by dogfights, things blowing up, basically what Jeb grew up on. Play these movies to get a 10% cost reduction on pilots.

- Documentaries: Scientist kerbals love documentaries about the solar system, plants, animals, and space exploration. They especially love the ones they wrote :) Play documentaries to get a 10% cost reduction on scientist recruitment.

** All recruitment costs 50 inspiration, unless you're playing a matching movie type.

The University comes in when you want an educated kerbal Scientist or Engineer. See the movie theater makes a kerbal think he knows what he's doing. A university makes sure he knows. The university has a science lab for the scientists to process research experiments.

To recruit a level 3 Scientist or Engineer requires 5000 education and 50 inspiration. Open Classes to start the education. Educating a new kerbal will take roughly 10 kerbin days.

The Flight school comes in when you want a pilot who crashes less than Jeb.

To recruit a level 3 Pilot requires 5000 flight experience and 50 inspiration. Open classes to start the training. Training a new pilot takes roughly 10 kerbin days.

Rent

Civilians generate Rent now! Earn 100 funds per civilian per day as they live in your apartments on your station, ship or base.

Robotic construction facilities

The laser drill now has an ore drill on it. The netherdyne smelters return as well. Combine the Cnc Mill, smelter, Construction drone, and laser drill to enable construction of bases or stations in situ without the need for a kerbal present (though you may want one or two to place survey stakes). You still need to find suitable ore deposits. In situ construction requires Extraplanetary Launchpads.

1.4 released!

1.4 updates Civilian Population for 1.0.x KSP (tested with 1.0: 1.0.x currently does not have significant changes that impact my testing)

I've allowed the use of Ore in the converters to serve as a commonly dug up resource which you can then extract other resources from with the Civilian Population Converters. Note that the Karbonite smelter is obsolete with stock fuel system. Other than that...

* The laser drill's converter will extract Water, Biomass, and Carbon Dioxide from ore. Stock Ore can be converted into rocket fuel so this assumes that the stock ore contains hydrocarbons which would convert into biomass and water.

* Civilian Population also assumes the stock ore contains metals which allows the original Ore-Metal-RocketParts resource path for Extraplanetary Launchpads. This should make locating your industrialized base simpler than before.

I'm taking a break from Kerbal Space program after this update, gettin into other games. I'll be watching to keep Civ Pop current with the KSP releases. Godspeed!

New in 1.3

New textures on pretty much everything

Some parts were sufficiently detailed to not receive new textures. Others got brand new texture work. Biosphere domes, civlian pods, farms all got reworked.

New IVAs for most of the parts

All of the pods got new IVAs except the movie theater and the airlock. The movie theater is because I don't have the plugin made for it. The airlock is because it operates with 2 doors so you can get in and out of a biosphere or biodome.

New part: MK2 Cruiser Landing Gear

https://imgur.com/cUWABEh.png

Balance updates

 

Rent generated from a single city now meets diminishing returns. Up to 50 civilians will get the full 100 rent per civilian. After that it drops off by 1 per extra civilian. By 150 civilians you will only receive 10 funds per civilian with each rent cycle.
Farms are re-balanced to have the correct energy consumption. The large biosphere farm will require about 12 Gigantor XL solar panels to keep powered up.
The large Biosphere farm now supports 70 kerbals (roughly)
The Biodome Farm Mark 2 now supports 70 Kerbals (roughly)
The substrate converter on the Laser Drill no longer creates extra water (previously it would produce 5x the water that was input into it)
The construction drone now produces 30 productivity at the cost of 5kw electricity per second
The biosphere airlock can now store science experiments. Go do that EVA you wanted to!
 

Bug fixes

The Construction Drone now properly works with 5.1.2 Extraplanetary Launchpads
The duplicated Regolith.dll is no longer needed. I added the KSPDependency information.
Extraplanetary Launchpads is added in the KSPDependency information. It is required to use the construction drone.
 

About the Movie Theater and why it does not have an IVA yet

This is because I do not have the necessary plugin created to play movies inside of it! This will come probably in 1.4 with the cruiser parts expansion.

New in 1.2

New civilian house and landing pads

Javascript is disabled. View full album
Jumbo Truss system

Facilitate the construction of huge generation ships with the jumbo truss system.

Javascript is disabled. View full album
Biodome and Biosphere system

Build your Type 1 civilization base with biodomes, and the generation ships with biospheres. The construction of a permanent city on a body without any atmosphere would require the use of biodomes to contain atmosphere.

Javascript is disabled. View full album
Cruiser landing gear

The cruiser landing gear was born out of necessity. When operating a base where the weight can change the base can shift angle slightly by several degrees, causing some parts to clip into the ground when they really shouldn't have. Landing gear is proven the only thing that prevents this kind of shifting. To make very well sure that this glitch never kills a base again I created the cruiser landing gear. With 3 meters of play and up to 6 meters off the ground your base will never clip into the ground ever again, at least unless you made it 1km wide, then I can't save you.

https://imgur.com/WgtNhqe.png

City and ship album

Javascript is disabled. View full album

=======================
Some FAQS

Is TAC Life support needed

No. If you have it this mod may make it easier to manage though. I balanced the civies to consume 1kg of food/water per day and .1kg of oxygen per day. These are in kerbin days (6 hours). Depending on how you've configured TAC life support your crew may use more or less resources. They are the same resources. Unfocused ship simulation should be compatible as I use Regolith and MKS uses it and MKS is compatible with TAC.

Do I need MKS?

No. It enhances the experience though, providing parts to expand your base or station with. MKS unfocused simulation is now compatible.

Other mods which may help the experience

Scansat (for resources)

Extraplanetary launchpads (for building huge cities on a planet)

Hangar Extender: For building Generation ships with the new parts in 1.2

Misc notes from older releases, not yet integrated with full post

Civilians are now full-fledged Kerbals, instead of being abstracted as resources
There are now two methods of growth
Linear growth by Civilians being ferried to your base -> A new arrival will arrive roughly every 85 days within Kerbin/Mun/Minmus SoI (contractor docks)
Logistic growth by Civilians spawning within the craft -> Based on the number of Civilians present; first positive then negative exponential growth (apartment complexes)
Civilian Growth is tracked by a new resource called "Civilian Growth Rate".  It can be found on the upper-right hand corner of the screen with the other resources.  It can be taken as a percent complete (0-1.00) until another Civilian arrives.  Most of the time, it should read 0.00 because Kerbals take a long time to arrive.
The mod is now life-support agnostic (due to Civilians now being Kerbals).  Tac and USI Life Support are both supported in Module Manager config files
Known Issues/Workarounds:

All modules with crew do not have internal spaces for the crews (and thus no portraits).  If you need to EVA, use the GUI or right-click menu.  For transfers, it's only the right-click menu at the moment.  But for all parts, the first couple of seats should have an IVA.
I highly suggest using Ship Manifest which has the above functionality.
When a Civilian is added to the crew, the portrait does not automatically appear, even with a part with an internal space.  Still trying to find the root-cause of that.  Restart the vessel (go to another one and come back) and this should be resolved.
Working on this after I get unfocused vessels working
Civilians only spawn/come to your bases when they are the active vessel.
Currently working on it.

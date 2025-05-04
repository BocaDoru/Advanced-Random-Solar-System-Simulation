# Advanced Random Solar System Simulation

* This Unity project extends the [Solar System Simulation project](#https://github.com/BocaDoru/Solar-System-Simulation) to generate entire solar systems procedurally, rather than just individual planets. It focuses on efficient simulation of a large number of celestial bodies and explores algorithms for creating diverse and interesting solar system configurations.

## Table of Contents

* [Related Project](#related-project)
* [Changes from Previous Project](#changes-from-previous-project)
* [Key Features](#key-features)
* [Procedural Solar System Generation](#procedural-solar-system-generation)
    * [Star System Generation](#star-system-generation)
    * [Planet Distribution](#planet-distribution)
    * [Moon Generation](#moon-generation)
* [Efficiency Considerations](#efficiency-considerations)
    * [Collider-Based Gravity](#collider-based-gravity)
    * [Performance Optimization](#performance-optimization)
* [Usage](#usage)
    * [Generation Parameters](#generation-parameters)
* [Technical Details](#technical-details)
* [Author](#author)

## Related Project

* This project builds upon the physics engine and simulation framework developed in the [Solar System Simulation project](#https://github.com/BocaDoru/Solar-System-Simulation). Please refer to that project's README for details on:
    * Basic physics calculations (Newton's law of gravitation)
    * Simulation controls and user interaction
    * Core classes for celestial object representation

## Changes from Previous Project

* The most significant changes in this project are:
    * **Procedural Planet Generation Removed:** Individual planet generation (shape, texture) has been replaced with a system for generating the overall structure of a solar system.
    * **Procedural Solar System Generation Added:** Algorithms are implemented to create star systems with varying numbers of planets, orbital distances, and moon/ring systems.
    * **Efficiency Focus:** Increased emphasis on performance to simulate a larger number of celestial bodies simultaneously.
    * **Simplified Visuals:** Planets are represented by simple spheres to improve performance.

## Key Features

* **Procedural Solar System Generation:** Generates complete solar systems with multiple planets, moons or rings and asteroid belts.
* **Efficient Gravity Simulation:** Utilizes a collider-based approach to approximate gravitational influence, significantly reducing calculation overhead.
* **Large Body Simulation:** Optimized to handle simulations with a large number of celestial bodies (up to 20000).
* **Random Collisions:** Implements basic collision detection and handling between celestial bodies.

## Procedural Solar System Generation

* This project focuses on the procedural generation of the *structure* of a solar system, rather than the visual appearance of individual planets.
* The solar system structure can be manipulated by changing the **Celestial Body Settings** from unity editor. Those settings are:
  * **Inner Rocky Planets:** used to generate random inner rocky planets. Those planets typicaly are smaller, orbit closser to the sun and have a small number of moons or small rings.
  * **Outer Gas Giants:** used to generate random gas planets. Those planets typicaly are larger, orbit further to the sun and have a large number of moons or bigger rings.
  * **Asteroid Belt:** used to generate random asteroid belts. Those are small celestial bodys and they orbit in the middle of a solar system.
  * **Dwarf Planets:** used to generate random dwarf planets. Those planets are similar to the inner rocky planets, but they have a more elliptic orbit that intersects with other plenets or asteroid belts.
  * **Kuiper Belt:** used to generate Kuiper belts. Those are simalar to the asteroid belts but are found to the end of a solar system.
    
* Each planet have a chance to generate moons or rings. Those are generated based on the planet parameters and will be presented in the [Moon Generation](#moon-generation) and [Ring Generation](#ring-generation) section.
  
* **Celestial Body Settings** contains:
  * **N:** the number of this celestial body type.
  * **Name Lenght:** the name lenght for this celestial body type, used to differentiate between celestial body types.
  * **Reach Bounds:** the distance interval from the sun where this celestial body type can be found.
  * **Mass Bounds:** the mass interval in logarithmic scale.
  * **Speed Error:** the speed error, a small error result in more circular orbits and a bigger error result in a more elliptic orbit.
  * **Velocity Normal Vector:** the *Up Vector* for the orbital velocity. Typicaly all the celestial bodies of a gravitational system orbits in the same direction.
  * **Plan Normal Vector:** the *Up Vector* for the orbital plan. Typicaly all the celestial bodies of a gravitational system orbits in the same aproximate plane.
  * **Min Distance:** the minimum distance between 2 planets.
  * **Satelit Location:** the distance interval where a satelite(moon or ring) can generate.

### Celestial Bodies Generation

* All celestial bodies have the same generation algorithm, the differences are made by the initial settings.
* The mass is a random power of 10 from **mass bounds**.
* The smaller planets are distributed in the first half of **reach bounds**, and the bigger planets in the second half. Further noted with **(down, up)**.
* A random point is selected form a **unit sphere**, is projected on the **orbital plane** and normalize to give the generated planet vector. This vector is scaled with a random value from (down, up) interval and translated to the parent planet position.
* If another planet is in the minimum distance this process is repeated until a valid position is found or the maximum number of tries is reach.
* The planet velocity is calculated as:
  
  $$\vec{v_0}=\vec{V_{pp}}+\sqrt{\frac{G M}{|\vec{p} - \vec{P_p}|}} e_{rr} ({\frac{\vec{p}-\vec{P_p}}{|\vec{p}-\vec{P_p}|}}\times{\vec{v_n}})$$
  
  * $$\vec{v_0}=$$ initial velocity vector.
  * $$\vec{V_{pp}}=$$ parent planet velocity vector.
  * $$G=$$ gravitational constant $$G=6.67428^{-11}$$.
  * $$M=$$ parent plenet mass.
  * $$\vec{p}=$$ position vector.
  * $$\vec{P_p}=$$ parent planet position vector.
  * $$e_{rr}=$$ a random value from speed error interval.
  * $$\vec{v_n}=$$ velocity normal vector.

### Moon Generation

* For a moon to generate the parent planet need to have a gravitational atraction that suports this, there is 3/5 chance to generate a moon if this condition is valid.
* Each planet generate a new **Celestial Body Settings** for it's moons. Those settings are:
  * N= a random value in the interval (1, $$\log_{100} m$$), where m is the mass of parent planet.
  * Name Lenght= 2.
  * Reach Bounds= $$SatelitLocation*maxDistance$$, where *SatelitLocation* is from parent planet and *maxDistance* is the maximum distance where the orbit is stabile.
  * Mass Bounds= (minMassBound/2,$$\log_{100} m$$), where minMassBound is the lower interval value of Mass Bounds for the parent planet.
  * Speed Error= a random value from (0, 0.05).
  * Velocity Normal Vector= a random point on a unit sphere.
  * Plan Normal Vector= Velocity Normal Vector.
  * Min Distance= NaN.
  * Satelit Location= (0.1, 0.9).

### Ring Generation

* For a ring to generate the parent planet need to have a gravitational atraction that suports this and the mass to be larger than $$10^{15}$$, there is 2/5 chance to generate a ring if those condition are valid.
* Each planet generate a new **Celestial Body Settings** for it's rings. Those settings are:
  * N= a random value in the interval (1, $$\log_{10} m$$) and multiplied by 1000, where m is the mass of parent planet.
  * Name Lenght= 3.
  * Reach Bounds= $$SatelitLocation*maxDistance$$, where *SatelitLocation* is from parent planet and *maxDistance* is the maximum distance where the orbit is stabile.
  * Mass Bounds= (minMassBound/3,$$\log_{1000} m$$), where minMassBound is the lower interval value of Mass Bounds for the parent planet.
  * Speed Error= a random value from (0, 0.07).
  * Velocity Normal Vector= a random point on a unit sphere.
  * Plan Normal Vector= Velocity Normal Vector.
  * Min Distance= NaN.
  * Satelit Location= (0.1, 0.9).

## Efficiency Considerations

* This project prioritizes the efficient simulation of a large number of celestial bodies.

### Collider-Based Gravity

* For better performance i used a collider-based method that simulates **gravitational fields**.
* The collider is a sphere with radius $$r=\sqrt{m*G*10^5}$$

### Performance Optimization

* What other optimization techniques did you use?
    * Object pooling?
    * Spatial partitioning?
    * Multi-threading?
    * Other tricks?

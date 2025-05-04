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

##   Key Features

* **Procedural Solar System Generation:** Generates complete solar systems with multiple planets, moons or rings and asteroid belts.
* **Efficient Gravity Simulation:** Utilizes a collider-based approach to approximate gravitational influence, significantly reducing calculation overhead.
* **Large Body Simulation:** Optimized to handle simulations with a large number of celestial bodies (up to 20000).
* **Random Collisions:** Implements basic collision detection and handling between celestial bodies.

##   Procedural Solar System Generation

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
  * **Plan Normal Vector:** the *Up Vector* for the orbital plan. Typicaly all the celestial bodies of a gravitational system orbits in the same aproximate plane.
  * **Plan Normal Vector:** the *Up Vector* for the orbital velocity. Typicaly all the celestial bodies of a gravitational system orbits in the same direction.
  * **Min Distance:** the minimum distance between 2 planets.
  * **Satelit Location:** the distance interval where a satelite(moon or ring) can generate.

### Celestial Bodies Generation

* All celestial bodies have the same generation algorithm, the differences are made by the initial settings.
* The mass is a random power of 10 from **mass bounds**.
* The smaller planets are distributed in the first half of **reach bounds**, and the bigger planets in the second half. Further noted with **(down, up)**.
* A random point is selected form a **unit sphere**, is projected on the **orbital plane** and normalize to give the generated planet vector. This vector is scaled with a random value from (down, up) interval and translated to the parent planet position.
* If another planet is in the minimum distance this process is repeated until a valid position is found or the maximum number of tries is reach.
* The planet velocity is calculated as: $$\vec{v_0}=\vec{V_{pp}}+\sqrt{\frac{G M}{|p - P_p|}} e_{rr} (\times{\hat{p-P_p}}{v_n})$$.

###   Moon Generation

* Do you generate moons around planets?
* If so, how do you determine:
    * The number of moons per planet?
    * The orbital distances of moons?
    * The sizes/masses of moons?

##   Efficiency Considerations

    This project prioritizes the efficient simulation of a large number of celestial bodies.

###   Collider-Based Gravity

* Explain your collider-based approach.
    * Instead of calculating the gravitational force between every pair of objects, you use colliders to represent gravitational fields.
    * This significantly reduces the number of force calculations.
* How does it work?
* What are the trade-offs in terms of accuracy?

###   Performance Optimization

* What other optimization techniques did you use?
    * Object pooling?
    * Spatial partitioning?
    * Multi-threading?
    * Other tricks?

##   Usage

###   Generation Parameters

* Explain the parameters that users can control to influence the generated solar systems.
    * Number of planets
    * Distribution of planet sizes
    * Density of moons
    * etc.

##   Technical Details

* **Physics Engine:** Briefly summarize the core physics principles (Newtonian gravity) and refer to the other project's README for more details.
* **Random Number Generation:** Specify the random number generators you used and why.
* **Data Structures:** Mention any specific data structures you used for efficiency (e.g., lists, arrays, trees).

##   Author

* Boca Ioan Doru

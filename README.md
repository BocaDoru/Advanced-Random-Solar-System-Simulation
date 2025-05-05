# Advanced Random Solar System Simulation

* This Unity project extends the [Solar System Simulation project](#https://github.com/BocaDoru/Solar-System-Simulation) to generate entire solar systems procedurally, rather than just individual planets. It focuses on efficient simulation of a large number of celestial bodies and explores algorithms for creating diverse and interesting solar system configurations.

## Table of Contents

* [Related Project](#related-project)
* [Changes from Previous Project](#changes-from-previous-project)
* [Key Features](#key-features)
* [Procedural Solar System Generation](#procedural-solar-system-generation)
    * [Celestial Bodies Generation](#celestial-bodies-generation)
    * [Moon Generation](#moon-generation)
    * [Ring Generation](#ring-generation)
* [Efficiency Considerations](#efficiency-considerations)
    * [Why Collider-Based Gravity?](#why-collider-based-gravity)
    * [How Collider-Based Gravity Works](#how-collider-based-gravity-works)
    * [Performance Improvement Analysis](#performance-improvement-analysis)

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

* Each celestial body has a radius of $$\log_{100} m$$, where m is the mass of the planet.

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
  * N= a random value in the interval (1, $$\log_{100} m$$ ), where m is the mass of parent planet.
  * Name Lenght= 2.
  * Reach Bounds= $$SatelitLocation*maxDistance$$ , where *SatelitLocation* is from parent planet and *maxDistance* is the maximum distance where the orbit is stabile.
  * Mass Bounds= (minMassBound/2, $$\log_{100} m$$ ), where minMassBound is the lower interval value of Mass Bounds for the parent planet.
  * Speed Error= a random value from (0, 0.05).
  * Velocity Normal Vector= a random point on a unit sphere.
  * Plan Normal Vector= Velocity Normal Vector.
  * Min Distance= NaN.
  * Satelit Location= (0.1, 0.9).

### Ring Generation

* For a ring to generate the parent planet need to have a gravitational atraction that suports this and the mass to be larger than $$10^{15}$$, there is 2/5 chance to generate a ring if those condition are valid.
* Each planet generate a new **Celestial Body Settings** for it's rings. Those settings are:
  * N= a random value in the interval (1, $$\log_{10} m$$ ) and multiplied by 1000, where m is the mass of parent planet.
  * Name Lenght= 3.
  * Reach Bounds= $$SatelitLocation*maxDistance$$, where *SatelitLocation* is from parent planet and *maxDistance* is the maximum distance where the orbit is stabile.
  * Mass Bounds= (minMassBound/3, $$\log_{1000} m$$ ), where minMassBound is the lower interval value of Mass Bounds for the parent planet.
  * Speed Error= a random value from (0, 0.07).
  * Velocity Normal Vector= a random point on a unit sphere.
  * Plan Normal Vector= Velocity Normal Vector.
  * Min Distance= NaN.
  * Satelit Location= (0.1, 0.9).

## Efficiency Considerations

* This project prioritizes the efficient simulation of a large number of celestial bodies.

### Why Collider-Based Gravity?

* In a traditional gravitational simulation, the gravitational force between every pair of celestial bodies must be calculated each frame. This results in a computational complexity of O(n²), where 'n' is the number of bodies. As the number of bodies increases, the number of calculations grows dramatically, quickly becoming computationally expensive and limiting the scale of the simulation.
* To address this limitation, a collider-based method was implemented to approximate gravitational influence and reduce the number of force calculations, enabling the simulation of significantly larger solar systems.

### How Collider-Based Gravity Works

* Instead of calculating the gravitational force between every pair of objects, this method uses colliders to represent the region of space where a celestial body exerts a significant gravitational influence.
* **Collider Radius:** Each celestial body is assigned a spherical collider. The radius (r) of this collider is calculated based on the body's mass (m) and the gravitational constant (G):

   $$r = \sqrt{m * G * 10^5}$$

* **Gravitational Influence:** When a celestial body is within the collider of another body, it experiences a minimum gravitational acceleration. This avoids calculating the precise force for every distant object.
* **Force Calculation:** The gravitational force is only precisely calculated between bodies that are within each other's colliders.
* **Position Update:** The position and velocity of each celestial body are updated based on the net gravitational force acting upon it, as described in the "Solar System Simulation" project.

### Performance Improvement Analysis

* The traditional method has a computational complexity of O(n²), meaning the number of force calculations increases quadratically with the number of celestial bodies (n).
* The collider-based method aims to reduce this complexity. Here's a simplified analysis:
  
1. **Simplified Model:**
   * Assume there are 'n' celestial bodies.
   * Assume 'k' of these bodies are "large" enough to have significant gravitational influence (and therefore large colliders).
   * On average, assume each large body's collider contains 'c' other bodies.
   * **Traditional Calculations:** $$\frac{n * (n - 1)}{2}$$ (approximately $$n^2$$)
   * **Collider-Based Calculations:**
   * Calculations between large bodies: $$\frac{k * (k - 1)}{2}$$
   * Calculations between large and small bodies within colliders: $$k * c$$
   * Total (Simplified): $$\frac{k * (k - 1)}{2}+k * c$$
              
2. **Average Case Estimation:**
   * Let:
      * $$n_{large}$$ = Number of large planets
      * $$n_{small}$$ = Number of smaller celestial bodies (asteroids, etc.)
      * $$n_{planets}$$ = Total number of planets
      * $$max_{mass}$$ = Maximum mass of a celestial body
      * Average number of moons per planet: $$avg_{moons} = \frac{log_{100}max_{mass}}{2}$$, and 3/5th of planets have moons.
      * Approximate number of ring particles: $$n_{particles} = 1000 * \frac{1 + log_{10}max_{mass}}{2} * \frac{n_large}{10}` (assuming 1/10th of large planets have rings)
      * Total number of celestial bodies(plus the star):

      $$n_{total} = 1 + n_{planets} + n_{small} + n_{particles} + \frac{3}{5} * n_{planets} * avg_{moons}$$

      * **Number of Calculations (Approximation):**
         1.  **Large Body Interactions:** the number of interactions between large bodies O($$k^2$$).
         2.  **Large Body - Small Body Interactions:** the number of interactions between large-small bodies O(k * n).
         3.  **Small Body - Small Body Interactions:** the number of interactions between small-small bodies O(1).

3. **Complexity Comparison:**
   
   * The traditional method has a complexity O($$n^2$$) and the Collider-Base method O($$(k^2+k) * n$$).
   * If the number k is a small constant the complexity becomes O(n).

4. **Examples with Numbers (for more examples go to [desmos calculator](#https://www.desmos.com/calculator/pkrc2ya3gt):**

   * **Example 1:**
      * 5 large planets, 0 asteroids, max_mass = 10^16
      * Estimate $$n_{total}=4300
      * Traditional Calculations: $$n_{total}^2=18490000$$
      * Collider-Based Calculations: $$(n_{large}^2+n_{large}) * n_{total}=129000$$
      * Performance Improvement: $$\frac{Traditional-Collider}{Traditional} * 100%=\frac{18490000-129000}{18490000} * 100%=99.30%$$      
   * **Example 2:**
      * 10 large planets, 1000 asteroids, max_mass = 10^17
      * Estimate $$n_{total}=10000
      * Traditional Calculations: $$n_{total}^2=10000^2=100000000$$
      * Collider-Based Calculations: $$(n_{large}^2+n_{large}) * n_{total}=1100000$$
      * Performance Improvement: $$\frac{Traditional-Collider}{Traditional} * 100%=\frac{100000000-1100000}{100000000} * 100%=98.90%$$     
   * **Example 3:**
      * 11 large planets, 10 small planets, 10000 asteroids, max_mass = 10^17
      * Estimate $$n_{total}=20000
      * Traditional Calculations: $$n_{total}^2=20000^2=400000000$$
      * Collider-Based Calculations: $$(n_{large}^2+n_{large}) * n_{total}=2640000$$
      * Performance Improvement: $$\frac{Traditional-Collider}{Traditional} * 100%=\frac{400000000-2640000}{400000000} * 100%=99.33%$$

5. **Limitations:**

   * This method comes with some tread-offs:
      * **Lower Accuracy:** the Collider-Based Method comes with some accuracy issues, the collider radius limits the interaction by ignoring far away objects, this can cause some issus for long time simulation.
      * **Collider Interaction:** each collider uses some resources for detecting collision and for a very large number of objects this can affect negatively the simulation.
      * **Simpler Systems:** for simpler or mostly large planets solar systems the over all complexity will also be O($$n^2$$). The performance is relevant in complex systems with relative small number of large planets and a very large number of small objects.

VAR current_knot = "start"
VAR index = 0
->start
=== start ===
In order to progress through this dialogue, press [space]. 
Hello there, I am your Guide in this interactive demonstration. All of my sources are accessible from the main menu.
To move around the world, click on a tile with your mouse. 
You can zoom in and out using the up and down arrow keys. 
Clicking the right mouse button will, once we have reached them, disable the buildings, trees, and me so you can better see the layout of the region you are in.
You can select responses to this dialogue by clicking on the corresponding box:
* [I understand] -> define

= define
The concept of walkable cities is a broad one. The article "What is a walkable place," by Ann Forsyth, explains that "walkability" is to describe many different things, such as how walking is enabled, the effect on the liveliness and sociability of an environment, or generally to refer to better urban places. 
She identifies walkable environments as those that are traversable and compact, safe from both crime and traffic, and attractive to pedestrians because they are “lively and sociable.”
Walkable cities have numerous benefits, but there are a few key ones that stand out. They generally have lower maintenance costs than suburbs, they generate more revenue than less walkable spaces, they are better for the environment, and they are widely viewed as a more pleasant way to live. 
-> ready

= ready
Are you ready to start?
+ [Yes] 
    ~ current_knot = "suburb"
    ~ index = 1
    -> END
+ [No] 
    ~ current_knot = "start.ready"
    -> END

=== suburb ===
The first scenario we will look at is a representation of the way that some cities are organized. The layout here consists of interconnected streets and has long and narrow blocks.
We are only looking at one section of a neighborhood, but typical suburbs usually consist of multiple sections with this shape. The streets have one access point to a larger road that allows for traveling between sections.
Look around for a bit and tell me what you observe about the ease of getting between any two places on the map. The green tiles represent land that would otherwise be occupied by buildings and houses.
~ current_knot = "suburb.explored"
-> END

= explored
The biggest issue with suburbs organized in this fashion is the difficulty from getting between two points. If you are halfway down a block and want to move just a few tiles over to the next block, you have to travel all the way around the block to get there. 
This is an even larger problem in the case of the top left side of this environment. Here, if you wish to access the street (which has no sidewalk), you must walk all the way out of the section of the suburb and then around to reach it. 
The path is so long that the path finding algorithm for player movement struggles to calculate a path. 
Cities organized in this way make it difficult for anyone, driving or, especially, walking, to move between some places that are very close to each other spatially.
-> progress

= progress
Would you like to move on to the next scenario?
+ [Yes, take me to the next scenario please] 
    ~ current_knot = "suburb_fix"
    ~ index = 2
    -> END
+ [No, I would like to look around a little more first] 
    ~ current_knot = "suburb.progress"
    -> END

=== suburb_fix ===
The second scenario we will look at aims to fix the problems shown by the first one. Again, this environment is a representation of a different possible way of organizing city streets. 
This scenario is more suited for higher density living arrangements, but the principles of a more grid like organization can be applied elsewhere.
Look around for a bit and tell me what you observe about the ease of getting between any two places on the map. A reminder that the green tiles represent land occupied by buildings.
~ current_knot = "suburb_fix.explored"
-> END

= explored
A grid-like organization allows for much easier access to spaces close to each other. Rather than having to walk long distances to get around obstacles, the average path between any two points is much shorter. This makes this environment much more suitable for pedestrians who would otherwise have to walk much further. 
There is a downside with a grid-like organization. Much more of the land has to be used on roads. In a walkable city more focused on pedestrian-centric infrastructure, less land can be used by only needing to provide pedestrian access. 
-> progress

= progress
Would you like to move on to the next scenario?
+ [Yes, take me to the next scenario please] 
    ~ current_knot = "stripmall"
    ~ index = 3
    -> END
+ [No, I would like to look around a little more first] 
    ~ current_knot = "suburb_fix.progress"
    -> END

=== stripmall ===
This scenario is a little more complicated than the representation of possible city layouts. It demonstrates how different spaces actually feel to move around in. In this scenario, we will analyze a typical strip mall. 
Rather than mixed use zoning, many suburbs in the U.S. have low density commercial zones near residential areas. 
Feel free to look around and think about any problems that make this location unwalkable. Come back to me when you’re done.
~ current_knot = "stripmall.explored"
-> END

= explored
Strip malls are poorly designed for pedestrian accessibility. Which of the top four problems do you want to learn more about? 
 * [The buildings are too far from the street] -> too_far
 * [The large parking lot] -> parking_lot
 * [Commercial zoning] -> commercial_zoning
 * [Not pedestrian friendly] -> not_pedestrian_friendly
 + [Move on] -> move_on

= too_far
Strip malls tend to place buildings very far from the street. This makes it difficult for pedestrians without cars to access the buildings. 
Although this scenario does not, many strip malls also have fences or barriers between different complexes making travel between complexes needlessly complicated. 
-> explored

= parking_lot
The parking lot causes a lot of problems for pedestrians. Many parking lots are underutilized for the majority of the time. 
Excessive parking in California is a result of minimum parking laws. Nugent explains these laws created mandates that required developers to create a minimum amount of parking spaces when developing new buildings, both residential and commercial.
California, last year, passed a new law that repealed this requirement, but many metropolitan areas in the United States still have them.
-> explored

= commercial_zoning
Restrictive zoning laws create different requirements for commercial and residential development that makes it difficult to build mixed-use areas. This moves residents further away from areas of commerce and reduces their access to certain resources. 
Abramson found that in the top 35 metropolitan areas of the United States, 98.8% of the land has restrictive zoning that prohibits mixed-use development. 
-> explored

= not_pedestrian_friendly 
There is rarely adequate pedestrian support within the parking lots of strip malls. Many strip malls lack internal sidewalks and make everyone, including those with cars, walk through the parking lot to get to their destination.
Abramson talks about the important distinction between roads and streets: roads are a method of travel between two places, and streets are a destination for commerce and residence. Strip malls are usually accompanied by “stroads,” which are dysfunctional combinations of the two.
Stroads usually have many lanes, which is usually accompanied by higher speed limits. This makes them less safe for both drivers and pedestrians walking alongside them. Crosswalks can be very far away which makes it more difficult to access stores on the other side of the “stroad.”
-> explored

= move_on
The next scenario we will look at offers some marginal improvements to this one. The buildings are much closer to the street and parking lots are relegated to vertical parking structures.
-> progress

= progress
Would you like to move on to the next scenario?
+ [Yes, take me to the next scenario please]
    ~ current_knot = "improvement1"
    ~ index = 4
    -> END 
+ [No, I would like to look around a little more first]
    ~ current_knot = "stripmall.progress"
    -> END

=== improvement1 ===
This second scenario is modeled after a typical commercial area in a small US city. In areas with higher population density, mixed use zoning is more common. 
Feel free to look around. Think about how some strip mall problems are not problems here, and also think about what other improvements could be made. 
~ current_knot = "improvement1.explored"
-> END

= explored
This area is still not very walkable, but it is more friendly to pedestrians than strip malls are. Which features would you like to learn about?
* [Parking structures instead of large parking lots.]-> parking_structures
* [Wide roads.]-> wide_roads
* [Alternatives to car centricity.]-> car_centricity
+ [Move on ]-> move_on

= parking_structures
Parking structures are complicated. Compared to sprawling parking lots, they are much more efficient with their land use. However, parking structures necessitate infrastructure for cars, and usually center it. They incentivize the use of private, commercial vehicles to the detriment of other solutions. This can pose a problem to walkability.
-> explored

= wide_roads 
As before, this scenario still has stroads. 
Wide multi-lane roads like this one are often justified as a solution to city traffic congestion. This is untrue, and more lanes usually make traffic worse. 
When traffic is light, wide roads means higher speed limits, and more of a risk to pedestrians. 
-> explored

= car_centricity
Part of building a truly walkable place is incentivizing transportation solutions outside of just personal commercial vehicles, or cars. Public transportation is a common solution that reduces the number of vehicles on the road. 
Another common solution is bike lanes. By making dedicated bike infrastructure, we can allow pedestrians to ride a bike to work rather than getting into a massive car and taking up space for transportation. 
The problem with these solutions is that they require proximity. This is one of the reasons why multi-use zoning is so important. By having people live closer to the things they need, we can greatly reduce the amount of drivers.
-> explored

= move_on
The difference between strip malls and this scenario are drastic, and the space immediately feels more comfortable. However, it still lacks a lot of the components of a truly walkable place.
-> progress

= progress
Would you like to move on to the next scenario?
+ [Yes, take me to the next scenario please] 
    ~ current_knot = "improvement2"
    ~ index = 5
    -> END
+ [No, I would like to look around a little more first]
    ~ current_knot = "improvement1.progress"
    -> END

=== improvement2 ===   //(large street medium-density mixed use)
This next scenario is similar to the previous one, but offers solutions to a lot of the problems that the last one had. There is mixed use zoning, but also infrastructure that makes the environment feel safer, nicer, and ultimately more walkable.
Look around. Think again about the problems with the previous scenario. How has this solution fixed them? What problems remain?
~ current_knot = "improvement2.explored_intro"
-> END

= explored_intro
This is a much more walkable space. It makes being a pedestrian more comfortable, and safer, while also making the environment look and feel nicer. ->explored
= explored
Which features would you like to learn about?
* [Bike lane / protected sidewalk] -> bikelane_sidewalk
* [Greenery] -> greenery
* [On-street parking] -> street_parking
* [Streets, not roads] -> not_roads
+ [Move on] -> move_on

= bikelane_sidewalk 
Dedicated bike infrastructure makes using bikes for transportation a much more attractive option. In this scenario, there are not only separate bike lanes, but those lanes are protected from the car portion of the road. This makes them much safer than traditional “share the road” bike lanes. 
Sidewalks protect pedestrians by separating them from moving vehicles. By recessing the sidewalk, in this case behind a bike lane that is also recessed, pedestrians are less likely to be hit by cars that might swerve off the road.
-> explored

= greenery 
Greenery, in the form of trees and sometimes grass, serves a multitude of purposes in a city.
The first noticeable effect is that trees make environments more aesthetically pleasing. Concrete and asphalt can become dull, and greenery is one of the factors that Forsyth determines can make a place feel more walkable.
Beyond just looking better, greenery can help filter harmful pollutants out of the air. A report by Putol on a Georgia State University study found a “37% reduction in soot and a 7% decrease in ultrafine particles”. 
-> explored

= street_parking 
On-street parking provides a few benefits. It acts as another barrier between pedestrians and moving vehicles. On-street parking also slows traffic and creates some parking spaces for those still using cars. 
From a walkability standpoint, on-street parking is a massive improvement on large, sprawling parking lots.
-> explored

= not_roads
This scenario actually has streets instead of stroads. The streets are narrow, and are dedicated to being a place for people to go rather than a means of transporation. 
The greenery, on-street parking, and bikelanes make this a more appealing place to be as a pedestrian, and therefore a place to go to.
-> explored

= move_on
This scenario is overall a much more walkable space than either the strip mall or the higher density shopping area. 
This concludes this demonstration. Feel free to keep looking around and imagine what our cities would feel like if they were designed to be walkable.
-> END

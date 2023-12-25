# Design guides for an data structures and algorithms API
Although this code is primarily to get experience implementing algorithms and not meant to be used as a library, I did 
get to form some ideas about how such a library should be designed. 

1. You need to decide on consistent construction idioms.
2. You need to decide on the cloning mechanism.
3. You need to decide on consistent conversion mechanisms.
4. You need to decide on which methods to implement as part of the interface, and which to implement as extension methods.
My view here is that the interface needs to be fairly minimal, and that extension mechanisms should generally be preferred
for methods that are not core to the type. 
5. You need to decide how to deal with immutability. In addition to having ReadOnly interfaces, having types that are
actually immutable can make implementation of many features more straightforward, since you do not have to worry that 
the container's contents will change under your feet.
6. You need to consistently use the TryGet pattern.
7. You need to trace algorithms consistently, and have the output visible as examples.
8. You need to put guards consistently. (Or maybe this is not such a good idea?)
9. The four graph types need the same set of methods and algorithms where applicable.
10. Much more thought should be given to the unit tests structure:
    - To make it easier to write tests for set of implementers. 
    - To make it easier to have named test cases. 
    - To have more consistency among the tests. 

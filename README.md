
# SeansPanasonicMX70API
An api to programatically control a panasonic mx 70 vision mixer


Basically, the mx70 can be controlled via a serial interface. 
more info here https://seanwasere.com/seans-panasonic-mx-70-api/


The Baud rate i used was 38400.


The control codes i could figure out from trial and error were,



☻AFD:A1111♥           //audio level  A/B 0000 - FFFF

☻VAS:001♥

☻VAS:A0001♥

☻VAS:N01111111♥

☻VCC:A0001♥

☻VCG:A0001♥

☻VCY:A0001♥

☻VCP:A2♥              //video souce A/B 1-8  

☻VDA:N0111111♥

☻VDK:001♥             //toggles dsk

☻VDK:A0001♥

☻VDK:N0111111♥

☻VDL:001♥             //DSK set K Level 0-255  VDL:00-VDK:FF

☻VDL:A0001♥

☻VDS:001♥             //DSK Slice and Slope VDS:000 - VDS:FFF  1st 2 figits = Slice, 3rd gigit = slope. All 000 = full brightness 

☻VDS:A0001♥

☻VDZ:000♥

☻VEB:100♥

☻VFA:001♥

☻VFM:001♥

☻VFM:A0001♥

☻VFN:100♥

☻VKC:011111111111111111111♥

☻VKL:001♥

☻VKL:A0001♥           //☻VKL:011111111111111111111♥

☻VKS:A0001♥           //☻VKS:011111111111♥

☻VKO:000♥

☻VKR:011111111111111111111♥

☻VKS:001♥

☻VKT:100♥

☻VMA:001♥ //autotake  //☻VMA:011111111111♥

☻VMM:001♥ //lever     //☻VMM:011111111111♥

☻VMM:A0001♥

☻VMP:011111111111♥

☻VMW:011111111111♥

☻VSD:A0001♥           //☻VSD:011111111111♥

☻VWN:011111111111♥    //changes pattern, pattern id must exist otherwise it goes to nmext highest



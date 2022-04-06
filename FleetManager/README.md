<h1 align="left"> Fleet Manager </h1>

### Description

This application based on SOA (Service Oriented Architecture) exposing functionalities with Rest api endpoints as following.
Api endpoints can be used as followings.

To add a vehiche
~~~~
/api/vehicle
~~~~

To add a delivery point
~~~~
/api/deliverypoint
~~~~

To add a bag shipment
~~~~
/api/shipment/addBag
~~~~

To add a package shipment
~~~~
/api/shipment/addPackage
~~~~

To add package to bag
~~~~
/api/shipment/addPackageToBag
~~~~

To Query shipment
~~~~
/api/shipment/detail/{barcode}
~~~~

To Query single wrong delivery
~~~~
/api/shipment/wrongDelivery/{barcode}
~~~~

Sample usage and parameters can be found in /swagger/index.html

Project has been dockerized and can be run through docker compose.

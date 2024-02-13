# OVH Poc

**Full disclosure** : 
I did not take into account Authentication/Authorization, Exception handling, Entities validation, as it was not the main objective of this exercise.


# Brief description of the technical aspect of the proposed solution
I've made the choice to put in place a Microservices Architecture with Rabbitmq as a message/queue tool to ensure the communication between the 2 main services : 
  - Publisher API : OVHPoc.Publisher.Api 
  - Consumer API : OVHPoc.Consumer.Api

This conceptual choice was made to try and address the potential important concurrent load on the app (migration requests)

## Steps on how to run this application

The first main step is to get Rabbitmq up and running locally by executing this command:
![image](https://github.com/Taher-Abdeljaoued/OVHPoc/assets/73798429/1ad8a106-ec2c-4d8b-b16d-c670aa14706c)

## Both of the APIs must run simultaneously.

## Then you're ready to go. Not very complicated i hope !! 😵‍💫

#### Presentation of the app's Endpoints :

The publisher API **OVHPoc.Publisher.Api** proposes :
 - Endpoints to get mails from both providers AlmostMail and Merelymail by entering an email *(emails from provided datasets only)*
 - Endpoint to get the current status of the migration
 - Endpoint to generate a Migration Request by entering an **email** and the **sourceProvider** which can be either **AlmostMail** or **MerelyMail** -> That'll be added added to the queue and consumed by **OVHPoc.Consumer.Api**
![image](https://github.com/Taher-Abdeljaoued/OVHPoc/assets/73798429/f22698f7-0908-48d2-b170-932f3ed5c553)

### Migration from AlmostMail -> MerelyMail will insert the mailbox in MerelyMail sqlite database
Example of 2 AlmostMail mailboxes that were migrated to the MerelyMail database with their respective Mails and Folders.
  *Mailbox*
![image](https://github.com/Taher-Abdeljaoued/OVHPoc/assets/73798429/bc79b97b-b743-436c-9098-d88ade435208)

  *Folders*
  
![image](https://github.com/Taher-Abdeljaoued/OVHPoc/assets/73798429/ccec9507-d412-4ccc-851f-a7c1ded421e0)

  *Mails*
![image](https://github.com/Taher-Abdeljaoued/OVHPoc/assets/73798429/68909fea-b1cd-4bad-a05f-10607dbee4be)


### Migration from MerelyMail -> AlmostMail will generate a json file of the mailbox and saves locally in the solution : 
Example of some MerelyMail mailboxex that were migrated to AlmostMail format :

![image](https://github.com/Taher-Abdeljaoued/OVHPoc/assets/73798429/c53befb5-8a18-41cc-936a-bff61934a246)



## I think you're all set to go and try to request some migrations !  👊

# Foodtable .Net Web-Api

<img src="/image.png" width="400"/>


## What is Foodtable?

You know that feeling when you want to order something for lunch with your friends and one of them always has to write everything down?
Foodtable is an app that I programmed. With this app, you can create groups and add users. Users can add an order, which is then summarized in a PDF file in the frontend. This PDF file now shows all the orders in a group and from which user they are from.

## TODO Before Using API 
- Git Clone the Project
- [IMPORTANT] Build the Project(Click on the Foodtable.sln File)
- Go to the Appsettings.JSON file and change the DataBase Connection String you will find the Instruction there
- Save And start the Project

If you're starting the "rebuildDB.bat" script in the Shell Console it will delete and rebuild the Database !!!ATTENTION!!! YOU GONNA LOSE ALL DATA. 
To start script navigate in the Folder and open with Shell Console:

```bash
.\rebuildDB.bat
```
and the script will start


## User requests

[GET]
- Get a User 
- Get All Existing Users 
- Get All Groups Where User is Joined 
- Get All Active Groups Where User is Joined 
- Get All Orders from User 


[POST]
- Create A New User


[UPDATE]
- Update a User


[DELETE]
- Delete A User







## Group requests 

[GET]
- Get All Existing Groups 
- Get All User From a Group 
- Get a Active Group 
- Count All Users From A Group
- Get Group Order and whole UserOrders at the Date <br/>
Hint: Date Format to GET whole Group Order and UserOrders = " yyyy-MM-dd "
- Count All User Orders At the Date <br/>
Hint: Date Format to GET Counted User Orders= " yyyy-MM-dd "


[POST]
- Create Group
Hint: If you're Creating a Group let the GroupId null it's Random Generate a Id in the Backend.


[UPDATE]
- Put User in a Group 
- Update Group
Hint: Put the GroupId in the Body from the JSON to check if Group is existing


[DELETE]
- Delete Group
- Delete User From a Group 







## Order requests

[GET]
- Get All Existing Orders
- Get Only a Order
- Get All Exising User Orders 
- Get a User Order 

[POST]
- Create Order <br/>
Hint: If you're Creating a Order let the OrderId null it's Random Generate a Id in the Backend.

- Create User Order <br/>
Hint: If you're Creating a User Order let the UserOrderId null it's Random Generate a Id in the Backend.


[UPDATE]
- Update a Order <br/>
Hint: Let the UserId field in the JSON File empty

- Update a  User Order <br/>
Hint: Let the GroupId field in the JSON File empty


[DELETE]
- Delete a Order 
- Delete a User Order

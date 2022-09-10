# Foodtable .Net Web-Api

<img src="/image.png" width="400"/>


## What is Foodtable?

You know that feeling when you want to order something for lunch with your friends and one of them always has to write everything down?
Foodtable is an app that I programmed. With this app, you can create groups and add users. Users can add an order, which is then summarized in a PDF file in the frontend. This PDF file now shows all the orders in a group and from which user they are from.

## TODO Before Using API 
- Git Clone the Project
- Build the Project(Click on the Foodtable.sln file)
- Go to the Appsettings.JSON file and change the DataBase connection String you will find the Instruction there
- Save And start the Project

If you're starting the "rebuildDB.bat" script in the Shell Console it will delete and rebuild the Database !!!ATTENTION!!! ALL YOUR DATA WILL BE DELETED. 
To start script navigate in the Folder and open with Shell Console:

```bash
.\rebuildDB.bat
```
and the script will start


## User requests

[GET]
- Get a User 
- Get all existing users 
- Get all groups Where user is joined 
- Get all active groups where user is joined 
- Get all orders from a user 


[POST]
- Create a new user


[UPDATE]
- Update a user


[DELETE]
- Delete a user







## Group requests 

[GET]
- Get all existing groups 
- Get all users from a group 
- Get a active group 
- Count all users from a group
- Get group order and whole UserOrders at the Date <br/>
Hint: Date Format to GET whole GroupOrder and UserOrders = " yyyy-MM-dd "
- Count All user orders at the Date <br/>
Hint: Date Format to GET counted user orders= " yyyy-MM-dd "


[POST]
- Create group
Hint: If you're creating a group let the GroupId null it's Random generate a Id in the Backend.


[UPDATE]
- Put user in a group 
- Update group


[DELETE]
- Delete group
- Delete user from a group 







## Order requests

[GET]
- Get all existing orders
- Get only a order
- Get all exising user orders 
- Get a user order 

[POST]
- Create order <br/>
Hint: If you're creating a order let the OrderId null it's Random generate a Id in the Backend.

- Create user order <br/>
Hint: If you're creating a user order let the UserOrderId null it's Random generate a Id in the Backend.


[UPDATE]
- Update a order <br/>

- Update a user order <br/>


[DELETE]
- Delete a order 
- Delete a user order

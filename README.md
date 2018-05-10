**561_Project**

In this project I use asp 2.0 razor page 
implemented a AnimeCollection site:   https://animecollections20180509032341.azurewebsites.net/



For Demo You can use three account below:

Website Manager Account:
**compe561@manager.com**
Qwer1234.

Website Admin Account:
**compe561@admin.com**
Asdf1234.

Website User Account:
**CompeUser@com**
Abcd1234.

**(SeedData.cs) path: ~/Models/SeedData.cs Cotains some sample datas **

**Fuctionalities For EachRoles**

**User**: can see the status, read allowed anime contnet/ info

**Manger**: can decide which cotent is okay for User to see (partent Control)

**Admin**: can do anything 


My Database in Nomorilize Form looks Below

![alt text](https://github.com/leyulin/561_Project/blob/master/AnimeCollectionDb.png)


**Challenges**
I think the biggest change is mutiple DBcontext and inherit for pagemodel 
When is multiple Dbcontext  have to dotnet ef database update --Context xx
And DataBase seems Have to use "Unity of Work" to update base only one Dbcontext.

I have very complex 3NF at beginning, but So diffucit to do all contreller.cs and context and database.
So I change to much simple Forms

Azure is very changelles too, I used dacpac restore to Azure try almost 3 days. and Finally get it work.















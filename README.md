# Chest Royale
A Chest System like Clash Royale with Save mechanism and can deduct time passed from last save when opening again. <br/>
[Gameplay Trailer](https://drive.google.com/file/d/1RDp_EBn0DGD0POFzNWCAzlEGKtWEEBNb/view?usp=share_link) <br/>
[Playable Link](https://kishore-karic.itch.io/chestroyale) <br/>

# Important Mechanism
* Save mechanism with JSON to Save and Load Chest.
* Asynchronus code with Async Await for calculating Time.
* Server Time to avoid chesting with Device time.
* At Save take current time from Server and Save it and at Load take current time from Server and reduce it with Last Saved time to get Reamining time to calculate with Chest Unlocking time.

# Game Functionality
1. 4 different types of Chest.
2. Each with different Unlock time.
3. Only one Chest can be Unlocking at a time.
4. Maximum 2 Chests can be stored into Unlock queue we can edit it's limit too.
5. If the Chest in queue is unlocked next will be automatically start unlocking.
6. Calculating time Asynchronausly using Async Await.
7. Also can open with Gems if they meet the rquired Gems means.
8. Gems can be calculated like 1 Gem for 10 minutes.
9. All the Chests with current details and Currencies can be SAVED if only exit with Exit button.
10. Using JSON to Save and Load from directory path.
11. At opening time will automatically reduce time passed from last save and make Chest into next State (Unlock or Open)
12. Calculating time from SERVER to avoid Chesting with device time, So must have INTERNET connection.
13. Updating current status on UI like Chest status (Lock, Unlock or Open), Chest Type (Commomn, Rare, Epic or Legendary), Unlock Timer in h/m/s format.

# Design Patterns
* MVC for Chest.
* Scriptable Objects to Store different Chest details.
* Generic Singleton for Services.

# Screenshots
![Picture_1](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/31b34ac6-90da-47d9-a51a-c6e4d3deb724)
![Picture_2](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/c88d4b11-f380-447f-84ef-75ced87fa5f4)
![Picture_3](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/d44f9c7b-0701-4e30-a7b9-7a6bd1de48c3)
![Picture_4](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/e6342fcb-a822-4157-a7a3-ac9011fedab8)
![Picture_5](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/7179f61c-b4f4-4fa5-a15c-cfbdc4e2a79b)

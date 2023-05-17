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
![Picture_1](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/de5d3338-b5e5-49a8-99ba-0b8ea8f6082c) <br/>
![Picture_2](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/7beb6a33-cfe0-4d06-a1ad-9918dae46bd0) <br/>
![Picture_3](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/1e17ac11-a1ef-4cc4-8fc3-b8542c341e0e) <br/>
![Picture_4](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/d20e8e63-a854-45a4-bc0e-e4c234e98b5f) <br/>
![Picture_5](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/6ba0e28e-432d-4d50-83b8-9aa0ab0133b6) 

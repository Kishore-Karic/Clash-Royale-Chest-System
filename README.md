# Chest Royale
A Chest System like Clash Royale with Save mechanism and can deduct time passed from last save when opening again. <br/>
[Gameplay Trailer](https://youtu.be/QyO1bi01ajc) <br/>
[Playable Link](https://kishore-karic.itch.io/chestroyale) <br/>

# Important Mechanism
* Save mechanism with JSON to Save and Load Chest.
* Asynchronus code with Async Await for calculating Time.
* Server Time to avoid chesting with Device time.
* At Save take current time from Server and Save it and at Load take current time from Server and reduce it with Last Saved time to get Reamining time to calculate with Chest Unlocking time.

# Game Functionality
1. 4 different types of Chest each will have 3 tier.
2. Each Chests with different Unlock time.
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
13. Updating current status on UI like Chest status (Lock, Unlock or Open), Chest Type (Commomn, Rare, Epic or Legendary), Unlock Timer in d/h/m/s format.

# Design Patterns
* MVC for Chest.
* Scriptable Objects to Store different Chest details.
* Generic Singleton for Services.

# Screenshots
![Picture_1](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/6fdae896-84f6-46e3-b7c0-240340e3c62a)
![Picture_2](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/f9707ad0-694d-4f83-9d63-81ded2c2a185)
![Picture_3](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/1725be7e-967f-4214-b1ba-bfd4b558ed70)
![Picture_4](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/00af109c-ef54-41bd-b320-b6cb7fbe0296)
![Picture_5](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/e20cbecd-c3be-4009-8e26-1157ea61de4c)
![Picture_6](https://github.com/Kishore-Karic/Clash-Royale-Chest-System/assets/97879797/2b0c912d-df4b-4af1-86a9-5f68ac7b86bb)

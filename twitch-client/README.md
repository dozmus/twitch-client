twitch-client
===========================

twitch-client is a desktop application with the purpose of replacing most twitch website interactions for streamers.

### Features
* View broadcast details (viewers, latest followers, avg. fps, etc.)
* Update broadcast title/game
* Chat bot (and chat box)
* Stream preview
* Toggleable update timers

### How to Setup
* Run application
* Enter your twitch username
* Click the connect with twitch button
* Authorize the application (you may need to log into twitch before this, if you aren't already)
* You will be redirected to a custom page which will contain the response code which you must paste into the 'Twitch Response' field in the twitch-client.
* Click done

### Mini Explanations
* Low RAM mode - disables stream preview (saves a lot of RAM)
* Certain fields will be set to '?' if they are inaccessible.
* The chat box's user list's management options are disabled until the chat bot receives moderator status.
* To create a chat bot, create a new twitch account with the desired chat bot name, etc. Then retrieve the password for the twitch-client password field from this [site](http://twitchapps.com/tmi/) (format should be 'oauth:[code]', which should **all** be pasted into the field).
* The application's scope includes some currently unused features.

### Bot Commands
* Echo commands - commands which simply output text, i.e. an entry of 'trigger:echo' will result in a message of '!trigger' resulting in the bot typing 'echo'.
* Random notifications - notifications which are pasted periodically

### Requirements
* Internet connection
* .NET Framework 4.5

### Technical Details
* C# on .NET Framework 4.5
* Twitch API with Implicit Grant Flow

### License (Mozilla Public License, version 2.0) - Summary
* You must disclose source code if distributing this software and include a copy of the license and copyright notice
* You cannot hold me liable for any damages or use my trademarks (contributors' names, logos, etc.)
* You may: distribute, modify, sublicense, use commercially, etc.

### Contributors
* [Pure_](https://github.com/PureCS)
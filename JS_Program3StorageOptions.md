Program 3 Storage Considerations:

Option 1: Player Preferences

Player preferences is a good option for basic settings like mouse sensitivity or volume, but I wanted to work with a storage system that was more dynamic and extensive than just the built-in Unity storage mechanism. I have also used player preferences on past projects, so I wanted to try something new that I haven't learned before and that meant player preferences was out of the picture. Technically, player preferences can be used in conjunction with a different storage option, so it might make sense to incorporate it down the road.

Option 2: Basic .txt File

Using some sort of string delimiter, a basic .txt file could be an option. However, I feel there is some concern for things getting too messy with a .txt file. I have also written and read from .txt files in previous classes, so the idea of using it here seemed a little bit easy. I wanted to try something I have never done before, and so like player preferences, a basic .txt file was also eliminated as an option.

Option 3: JSON .txt File

A JSON .txt file, after doing some research, seemed like a really interesting option to learn. It works similarly to a .txt file, but it contains the information in a more organized and consistent format. Utilizing JSON functions within Unity means things are a bit more organized and standardized than with a standard .txt file. If someone attempts to edit the file, there is less of a chance of it being put in a corrupted state. Because progress is not a big deal, if players want to edit their data, there is no real reason to worry too much about preventing them from doing so. Encryption or compression techniques are not necessary for this project, so JSON worked out as the best choice.
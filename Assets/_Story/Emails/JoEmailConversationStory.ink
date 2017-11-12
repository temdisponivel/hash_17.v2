INCLUDE ../StoryVars.ink

~ temp jo_email = "__jo__@tempemail.com"
~ temp my_email = "__17__@tempemail.com"
~ temp current_day = GET_VAR_VALUE(CURRENT_DAY)
~ temp day_one_investigated = GET_VAR_VALUE(DAY_ONE_INVESTIGATED)

MSG ID:   151DWEE81W65GC189EX15X
SENDER:   {jo_email}
RECEIVER: {my_email}
SUBJECT:  Interesting stuff
CONTENT:

{   
    - current_day == 1: 
        -> DAY_ONE
    - current_day == 2:
        -> DAY_TWO
    - current_day == 3: 
        -> DAY_THREE
    - else:
        -> WRONG
}

== WRONG ==
WRONG DAY
-> END

== DAY_ONE ==
<>
{jo_email} says:
    Hey, everything alright? I bet this will cheer you up a bit. I was scavaging the zone the other day and happen to found a old computer, nothing particularly interesting. Usually I'd just create suveniers out of them, as you know. But when I got a look inside it, I noticed that it had been modified, better CPU, more memory, even a GPU. Which, as you know, is quite unusual. So I tried to boot it up and see if I could recover any data. Oddly enough, it booted just find, even after all those years of radiation. The data was not intact though, as I could only recover so much of it. However, I did recover something quite intriging;
    It appears to be a transcript of an old presentation. It says something about a server containing all of the world's knowledge at the time. I'm setting up a SSH client on it so that you can login and see for yourself, so I won't say much here.
    Take a look and see what you think. I think it's just similar enough to the prophecy so that it's not a coincidance, but I can't be sure if it's even true. Anyway, take a look and tell me what you think.
    
    The SSH credentials are the default ones: guest guest
    IP: 59.28.0.6
    
    I asked around town about this and one of the elders told me about Job. He's an old hacker who spent his life trying to prove the prophecy. He is supposed to be a really good engeneer, but it just seems like an old, frustrated, depressive person who lives in a crappy and dirty appartment. I'll go talk to him to see if he has any usefull information.
    Bye

/*
{my_email} says:
    Hey Jo. Since when did you believe in prophecies and stuff you find in an old computer? I don't expect you to as cynical as me, but I didn't expect you to be so silly. Lucky enough, I have nothing else to do, so I'll take a look at it. But I can assure you it's nothing extraordinary. 
    And how are things around there? Everything alright? When are going to send me one of those suveniers?
    
    Anyways, talk to you tomorrow. Bye
*/

-> DAY_TWO

-> END

== DAY_TWO ==
<>
{jo_email} says:
    YOU WON'T BELIEVE THIS. I got hacked. All my data is gone! I went to Job's house and when I got back I found my computer wiped out! I knew I couldn't trust him the moment I put my eyes on him. He is just obssesed about anything prophecy-related. I went back to his appartment, but he wasn't there. You gotta help me find him, there's a lot of data about you there, including our emails and a log about "special" condition. 
    I think I know how we can find him. He is really paranoic, so he don't even use a modern cellphone, just an old GSM-only cellphone, I belive it's a NOKE 3585. There just one GSM antenna in town and not many old cellphones like that. Maybe we could get his location from that?! None of my deleted all of my hacking tools, so I cannot get it myself, but you can.
    Send me a map with his location. Please, you gotta help me! I owe you one and I promise I'll get you a suvenier if you do this for me.
    
/*
    Common, Jo. How could you be so careless? And why the fuck do you have a file with my info in it?? We fucked us both. I have no choice but to help you now. You'll have to give me 100 suveniers for this one.
    
    I'll get his location and send you. Get ready for a trip. Don't forget your gun and pills, you never know! Be carefull, if you die, it'll be just a matter of time untill this guy fuck me up, thanks to you.
*/

-> END

== DAY_THREE == 
<>
DAY THREE
->END
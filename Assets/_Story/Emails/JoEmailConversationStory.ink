INCLUDE ../StoryVars.ink

~ temp jo_email = "jo_021222@email.dpkr"
~ temp day_one_dream = GET_VAR_VALUE(DAY_ONE_DREAM)
~ temp day_two_dream = GET_VAR_VALUE(DAY_TWO_DREAM)
~ temp current_day = GET_VAR_VALUE(CURRENT_DAY)
~ temp my_email = GET_VAR_VALUE(MY_EMAIL)
~ temp my_name = GET_VAR_VALUE(MY_NAME)

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
This should never be shown!
-> END

== FIELD_TRIP ==
<>
You said he uses an old cellphone. Maybe we could track him down using the cellphone antenas to find him MNID; there's probably about 2 or 3 people still using these phones.

Get ready to a little trip, since I cannot get out of here, you'll have to go to him to confirm his location and get more intel.
-> END

== SHOULD_NOT_TRUST_YOU ==
<>
I knew I coudln't trust you! Let's just fix this
-> END

== BOREDOM_GETS_YOU ==
<>
Do you see what boredom gets you? I should have just ignored this nonsense! Now I got no choise but to fix this.
->END

== TRUST_IS_A_MISTAKE ==
<>
Everytime I trust someone, I get fucked. Let's just fix this mess already!
->END

== DAY_THREE ==
<>
[Message ID] d1s5dsdw170dw1ewfvge4w8dvf65e
[Created at] Fri, Dez 2, 2359
[From]       {my_email}
[To]         {jo_email}
[Subject]    This might interest you
[Content]:

{
- DAY_TWO_DREAM == DAY_TWO_DREAM_I_YELL:
    Calm down, mr perfect. Do I need to remind you about your little incident 2 years ago? If wasn't for me, you'd probably be dead.
    
    I don't need you to tell me that I fuck up. I know that already. Just help me get my fucking data back.

    And don't be such a drama queen. You know you can trust me! I'm the only one that knows about your "special condition". If you couldn't trust me, you'd be dead already.
    
    I'll get prepare. Pack my bag, my suit, my pills (you never know, right?!). I'll wait for the positions.
    Bye
    
- DAY_TWO_DREAM == DAY_TWO_DREAM_SOMEONE_YELL:
    I honestly though you'd be more upset. What a happy surprise. I'll get packed. Do you think I should get my pills? Well, I'll get anyway, you never know when you gonna need them.
    
    I'll wait for the positions.
    Bye.
}

-> DAY_TWO

== DAY_TWO ==

[Message ID] d1s5dsdw170dw1ewfvge4w8dvf65e
[Created at] Fri, Dez 2, 2359
[From]       {my_email}
[To]         {jo_email}
[Subject]    This might interest you
[Content]:

{
- DAY_TWO_DREAM == DAY_TWO_DREAM_I_YELL:
    Come on, Jo. When did you became so careless? How could you let this fucker who probably doesn't even know how to use a real cellphone hack you? If that happen, how can I trust these conversations? How can I be sure that someone isn't looking into your emails right now?
    
    He is probably looking into our emails right now!
    
    Let's get this data back. The more he has access to it, the more I'm compromised. You fucking idiot.
    
    {
    - day_one_dream == DAY_ONE_SUSPICIOUS_DREAM:
        -> SHOULD_NOT_TRUST_YOU
    - day_one_dream == DAY_ONE_TRUSTFULL_DREAM:
        -> TRUST_IS_A_MISTAKE
    - day_one_dream == DAY_ONE_BORED_DREAM:    
        -> BOREDOM_GETS_YOU
    }
    
    -> FIELD_TRIP
    
- DAY_TWO_DREAM == DAY_TWO_DREAM_SOMEONE_YELL:
    You fuck up this time, Jo. Our emails are on there, he probably knows who I am.
    
    I mean, I got no choise now. He know who I am, has access to our conversations.
    
    {
    - day_one_dream == DAY_ONE_SUSPICIOUS_DREAM:
        -> SHOULD_NOT_TRUST_YOU
    - day_one_dream == DAY_ONE_TRUSTFULL_DREAM:
        -> TRUST_IS_A_MISTAKE
    - day_one_dream == DAY_ONE_BORED_DREAM:    
        -> BOREDOM_GETS_YOU
    }
    
    -> FIELD_TRIP
}

<>
[Message ID] d1s5dsdw170dw1ewfvge4w8dvf65e
[Created at] Fri, Dez 2, 2359
[From]       {jo_email}
[To]         {my_email}
[Subject]    This might interest you
[Content]:

    You won't believe this! I my data is gone! My computer is just blank. What the fuck! I bet it was that son of a bitch...
    
    I went to his house, told him about what I found and got him really interested. He seemed like a new-born man. I think I might have woken up his old obssession. If I only knew...
    
    He asked me all these questions, my name, my email, how did I found the file. I should've seen this coming.
    
    At least I got something from him too...
    
    He is a very lonely (who isn't though?!), frustated man. Completely divorced from reality. He believes that the profecy is some kind of god-sent message to help human-kind reedem itself! (I mean, come on!).
    
    He spent most of his time on his chair now, by the looks (and smell) of it. I belive he hasn't cleaned himself in days. I bet he wouldn't last much longer before killing himself if wasn't for me. He uses an old xPhone V10, you know, the one that nobody uses in like 50 years or so. I don't think he is much into tech, just computers. He had a laptop with him, but by the crost of dirty on it, I'd say it hasn't been used in months. The microphone and camera are covered with black tape, and it was connected to a cable, so I don't think it has wireless capabilties (I'm not surprise though, neither do I).
    
    Please, you got help me! You got find T0m3 and get my data back. All that I have is there. My whole life, I can't afford to lose that data. Help a friend out!
    
    I know this is just the kind of investigative hacking you love anyway. And also, this guy is probably our best shot at finding the truth about this transcript. What you say?
    
-> DAY_ONE

-> DONE

== DAY_ONE ==
<>
\---------------------------------------------------- DAY_ONE -----------------------------------------------
[Message ID] d1s5dsdw15dw1d2w10wewdw57ewc112f
[Created at] Tr, Dez 1, 2359
[From]       {jo_email}
[To]         {my_email}
[Subject]    This might interest you
[Content]:


    I'm glad you I got you interested. I know you don't belive in profecies and neither do I. But this is just odd enough for me to be intriged.
    
    I've just finished the recovery of the SSD. There's nothing much, but maybe you'll find something.
    
    I did what you said and talked to some of the elders here. There's this old hacker, goes by the name of T0m3. From what I could gather, he spent his whole life trying to prove the profecy, but couldn't, obviously. If somebody knows something about this, it's him.
    
    I tried to contact him, but he didn't asnwered my emails, I'm not even sure if he is alive. I'll go to his house today.
    
    I've setup an old SSH on it so you can log in.
    
    Anyways... I've setup a SSH on the old computer. Here's the credentials. See if you can find something.
    
    Here's the IP address and password: 
    201.196.145.53 trustno1
    
    Talk to you later, bye. 
    
[Message ID] d1s5dsdw15dw1d2w10we4w8dvf9u3ff
[Created at] Wed, Dez 31, 2359
[From]       {my_email}
[To]         {jo_email}
[Subject]    This might interest you
[Content]:

{ 
- day_one_dream == DAY_ONE_SUSPICIOUS_DREAM:

    Well. I don't quite know what to say. I mean, how can we be sure that this is even true? We don't even have the whole transcript.
    But, to be honest, it got me interested. More because I have nothing else to do than because it's something amazing.
    
    Did you find anything else in the computer? Give me access to it and I'll take a look.
    
    I think it's a good idea for you to talk with some people, but be careful, people will think that you're crazy and you'll know what that means. 
    
- day_one_dream == DAY_ONE_TRUSTFULL_DREAM:

    Yeah, it sounds odd. And I know you wouldn't tell me something you didn't think is worth looking into.
    
    Just setup a SSH on the computer and I'll take a look on it. You should also check with some of the people there to see if anyone know something about this.
    
    I'll get started as soon as you give me access, but don't get too exiceted, it's probably nothing.
    
    Bye.
    
- day_one_dream == DAY_ONE_BORED_DREAM:
    
    Hi Jo. To be honest, it doesn't seem that promising and you know I don't believe in profecies... However, I also don't believe in coincidences.
    
    Sum that with the fact that I got nothing else to do, and you got me interested. Give me access to the computer and I'll take a look into it.
    
    Also, thank with some of the villagers there to see if they know something about this presentation.
    
    I'll get started as soon as I get access.
    
    
    Bye.
}

[Message ID] d1s5dsdw15dw1d2w10we4w8dvf8h4tt
[Created at] Tue, Nov 31, 2359
[From]       {jo_email}
[To]         {my_email}
[Subject]    This might interest you
[Content]:
    
        Hey {my_name}, how's going? I've come upon something interesting today. As you know, I often go just outside the zone to look for old computers.
    Today I found one, unamed, undated, but looked very old. I managed to recover some data from its solid state drive, and there is this file in there,
    that looks like a transcript of a presentation by some important person from way back. Here's what I could recover:
        
        "Today we've [lost_data] the [lost_data] we set out many [lost_data] ago. To [lost_data] and catalog all [lost_data]. I'm [lost_data] in front of [lost_data] to announce that we've [lost_data] single book. [lost_data] every single [lost_data] there is.
        All of this [lost_data] is being [lost_data] to our brand new [lost_data], one specially built for this. One that could endure any [lost_data] that human kind has seen. [lost_data] can penetrate this facility.
        We are, for all purpuses, creating a time capsule with all [lost_data] in the hopes that, if anything would happen [lost_data]."
        
        I know you don't believe in the profecy, but this sure sounds a lot like it. I thought that if anyone could find more about this would be you.
        I'll try to find more info on the computer and talk to some people about the profecy or this person in the transcript. Maybe one of the elders will know something.
        
        Talk to you soon,
        Bye.
        
-> END

-> DONE
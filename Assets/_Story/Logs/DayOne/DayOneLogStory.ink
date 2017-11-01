INCLUDE ../../DayOneVars.ink
INCLUDE ../../ExternalFunctions.ink

~ temp dream = GET_DAY_ONE_DREAM()
~ temp saw_email =  GET_DAY_ONE_SAW_EMAIL_BEFORE_LOG()
{
- dream == DAY_ONE_SUSPICIOUS_DREAM:
    -> SUSPICIOUS_DREAM
- dream == DAY_ONE_TRUSTFULL_DREAM:    
    -> TRUSTFULL_DREAM
- dream == DAY_ONE_BORED_DREAM:
    -> BORED_DREAM
}

== SUSPICIOUS_DREAM ==
    {log_status()}
    
    I think I'm getting too paranoic. Today I dreamed that my mother tried to kill me! Can't I even trust my dead mother? I shouldn't drink coffe before going to sleep, the cafeine might be enhancing my trust issues.
    
    {email_content_comment()} I'll take a look at it, but after this dream, I'd better not trust her too much, even though I have no choice considering what she knows.
-> END

== TRUSTFULL_DREAM ==
    {log_status()}
    
    Well, that was fun. I haven't had a nice dream in such a long time. Greate way to start a day. Till I look outside and see that it's raining. I don't even know I it bothers me so much. I can't go out anyway.
    
    Let's just hope the day will be as fun as my dream.
    
    Wow, I just read this and I sounded like a happy person. Very unlike me, no paranoia, no depression. I think this might have been the best log I wrote in a long time. Better not get used to it.
    
    Anyways
    
    {email_content_comment()}
    
-> END

== BORED_DREAM ==
    {log_status()}
    
    Today I'll probably kill myself - just kidding [or am I?].
    
    I can't take more of this, now even my fucking dreams are boring. Comon brain, the only reason I go to sleep is so that I can catch a break of this and you don't even come up with something nice? Or not nice? Just the same boring things over and over. What if I start taking drugs? Well, considering my "special condition", I better not.
    
    Anyways
    
    {email_content_comment()}
-> END

== function log_status ==
Tue, Nov 31. Raining. 68 KG. 12/7. Meds taken.

== function email_content_comment ==
{
- saw_email == true:
Jo sent me an email this morning. Something about a transcript related to the profecy. Even though there's no such thing as profecies, it got me interested. The file is quite old, from way before. It should be fun to investigate a little bit.
- else:
Jo sent me an email this morning. I didn't see it yet. But the subject says it's profecy-related so it's probably some bullshit. But, coming from Jo, it should be at least interesting.
}
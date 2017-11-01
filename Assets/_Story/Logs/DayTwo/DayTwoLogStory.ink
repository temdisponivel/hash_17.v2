INCLUDE ../../StoryVars.ink

VAR dream = 0
VAR saw_email = false 

~ dream = GET_VAR_VALUE(DAY_TWO_DREAM)
~ saw_email = GET_VAR_VALUE(DAY_TWO_SAW_EMAIL_BEFORE_LOG)

{
- dream == DAY_TWO_DREAM_I_YELL:
    -> I_YELL
- dream == DAY_TWO_DREAM_SOMEONE_YELL:
    -> SOMEBODY_YELL
}

== I_YELL ==
{log_status()}

Well, that felt good. It's not everyday you get to yell at somebody, even if in a dream.
{
- saw_email == true:
And after seing that email, it's like I can predict the future. I mean, what a fucking idiot. How can someone be so careless? I bet she is not pleased with my response. I gotta do this now. I mean, he has access to our conversations, probably some logs about me. I just can't take that risk. But, fortunally, now it's the fun part!
- else:
    I got an email from Jo, probably a follow up on the T0m3-profecy situation. I better go see that.
}
Let's get back to reality now, unfortunally.
-> END

== SOMEBODY_YELL ==
{log_status()}

Well, that brought up some childhood repressed memories alright.

{
- saw_email == true:
    Incredible how being yelled at, even if it's on a dream, can make you consider the other's people feelings. I mean, if Jo had fucked this hard any other day, I'd probably be cursing her right now. I guess remembering how it's to be wrong (which doesn't happen so often) gave me a little perspective. For better or worst.
- else:
    I got an email from Jo, probably a follow up on the T0m3-profecy situation. I better go see that. Let's hope she is not yelling at me for some reason.
}

-> END

== function log_status() ==
Cloudy. Fri, Dez 2. 68.3KG. 13/7. Meds taken.
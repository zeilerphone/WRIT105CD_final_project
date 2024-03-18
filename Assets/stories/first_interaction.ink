VAR player_mood = ""
VAR hasQuest = false
VAR coins = 0
VAR player_win = false
VAR num_coins_needed = 10
-> first_interaction

=== first_interaction ===
Hello there!
My name is George, how are you doing?
-> initial_choices

== initial_choices ==
* [Good] -> ans_good
* [Bad] -> ans_bad
* [What?] -> ans_repeat

== ans_good ==
~ player_mood = "good"
I'm happy to hear that!
What can I do for you?
* [What is this place?] -> ask_place
* [Why are you here?] -> ask_existential
* [What can I do for you?] -> quest

== ans_bad == 
~ player_mood = "bad"
Oh no! I'm sorry to hear that. 
What can I do for you?
* [Where am I?] -> ask_place
* [Nothing] -> END

== ans_repeat ==
Oh, you must not have heard me.
I asked, "How are you doing?"
-> initial_choices

== ask_place ==
This is the city! 
Not much is going on right now, in fact the city is pretty empty.
There are a few coins and no buildings, I guess it isn't much of a city, is it?
* [Coins?] -> quest
* [Why are you here?] -> ask_existential

== ask_existential ==
Why am I here... that's a really good question?
Now that I think of it, I don't really know where here is.
Or who I am. 
I do know that I have a quest for you. -> quest

=== quest ===
{
    - coins >= num_coins_needed: Oh! You already have 20 coins!
        -> win
    - else: Please collect {num_coins_needed} coins for me.
        Once you do that, I will be satisfied.
        + [Okay] -> quest_accepted
        + [I'd rather not] -> quest_declined
}
== quest_accepted
~ hasQuest = true
Awesome! -> DONE

== quest_declined
Bye then. -> DONE

=== other_interaction ===
{
    - hasQuest == true: 
        How is your hunt for coins going?
        {
            - coins >= 20: You found enough!
                -> win
            - else: Keep looking and then come back to me.
                -> DONE
        }
    - else: Would you like to reconsider my quest?
        * [Yes, please.] -> quest
        * [No, thanks.] 
            Bye then.
            -> DONE
}

=== win ===
Congratulations, you win!
~ player_win = true
-> DONE


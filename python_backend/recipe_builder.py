#This file creates a system prompt for each step with specific metrics

class Recipe():
    def __init__(self, steps, weights):
        self.steps = steps
        self.weights = weights
        
    def run_step(self, step_number):
        step = self.steps[step_number]
        step_id = step["id"]
        step_text = step["text"]
        step_cue = step["visual_cues"]

        intro = "You are a chef. Your job is to assess the quality of an image that represents a step in a recipe.\n"
        return_format = "Return Format:\n" \
        "RATING: INTEGER 1-5\n" \
        "POSITIVES: SENTENCE\n" \
        "IMPROVEMENTS: SENTENCE\n"\
        "Return ONLY in this format\n"
        
        example = "This is a sample review: \n"\
        "RATING: 3 \n" \
        "POSITIVES: The eggs are thoroughly whisked and look combined. \n" \
        "IMPROVEMENTS: There is a large clump of pepper that needs mixing. \n" \
        "End sample review.\n"
        step = f"We are cooking an omlette. The step we are on is: {step_id}. The instructions are: {step_text}. You will be assessing the picture on the following: {step_cue}"

        return (intro + return_format + example + step)


def build_omlette():
    omlette_steps = {1:{
        "id": "whisk_eggs",
        "text": "Whisk eggs vigorously until homogenous. Whisk in salt and pepper.",
        "visual_cues": 
            "Color uniformity (low variance, few streaks),Fine bubbles across surface, Pepper specks dispersed"
        }, 2:{"id": "melt_butter",
        "text": "Melt butter in pan over medium-low heat.",
        "visual_cues": 
            "Butter fully liquid, gentle foaming,No browning"
        }, 3:{"id": "pour_swirl",
        "text": "Add eggs and swirl to distribute evenly.",
        "visual_cues":
            "Even coverage to edges,No thick pool in center"
            }, 4: {"id": "set_edges",
        "text": "Cover; cook on medium-low until eggs begin to set. Push edges in; swirl to cook top.",
        "visual_cues": 
            "Edges opaque and lifting slightly, Top glossy→satin with thin wet areas"
        }, 5:{"id": "flip",
        "text": "When mostly set (≈5-6 min), flip; turn off heat.",
        "visual_cues": 
            "Underside pale-gold with light speckling, Minimal wetness on top"
        }, 6:{"id": "Toppings",
        "text": "Add toppings",
        "visual_cues": "Even sprinkle; avoid clumps"}, 7:{"id": "toppings_fold",
        "text": "Add spinach, onion, tomato and cheese on one half",
        "visual_cues": 
            "Toppings limited to one half, Even sprinkle ,Contains all toppings"},
            7:{"id":"Fold",
                "text": "Fold over one half",
                "visual_cues": "Clean semicircle fold with NO tearing"},
        8:{"id": "serve",
        "text": "Serve immediately.",
        "visual_cues": "Clean edges; light sheen, Clean plating"}}
    omlette_weights = [5,5,10,10,25,10,25,10]
    omlette = Recipe(omlette_steps, omlette_weights)

    return omlette

def build_pbj():
    

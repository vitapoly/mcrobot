//
//  UP_OpenEars.m
//  Unity-iPhone
//
//  Created by Michael Wang on 7/13/13.
//
//

#import "UP_OpenEars.h"

#import <OpenEars/PocketsphinxController.h> // Please note that unlike in previous versions of OpenEars, we now link the headers through the framework.
#import <OpenEars/FliteController.h>
#import <OpenEars/LanguageModelGenerator.h>
#import <OpenEars/OpenEarsLogging.h>


@implementation UP_OpenEars

@synthesize pocketsphinxController;
@synthesize fliteController;
@synthesize openEarsEventsObserver;
@synthesize slt;
@synthesize pathToDynamicallyGeneratedGrammar;
@synthesize pathToDynamicallyGeneratedDictionary;

- (void)dealloc {
	openEarsEventsObserver.delegate = nil;
}

// Lazily allocated PocketsphinxController.
- (PocketsphinxController *)pocketsphinxController {
	if (pocketsphinxController == nil) {
		pocketsphinxController = [[PocketsphinxController alloc] init];
        //pocketsphinxController.verbosePocketSphinx = TRUE; // Uncomment me for verbose debug output
#ifdef kGetNbest
        pocketsphinxController.returnNbest = TRUE;
        pocketsphinxController.nBestNumber = 5;
#endif
	}
	return pocketsphinxController;
}

// Lazily allocated slt voice.
- (Slt *)slt {
	if (slt == nil) {
		slt = [[Slt alloc] init];
	}
	return slt;
}

// Lazily allocated FliteController.
- (FliteController *)fliteController {
	if (fliteController == nil) {
		fliteController = [[FliteController alloc] init];
        
	}
	return fliteController;
}

// Lazily allocated OpenEarsEventsObserver.
- (OpenEarsEventsObserver *)openEarsEventsObserver {
	if (openEarsEventsObserver == nil) {
		openEarsEventsObserver = [[OpenEarsEventsObserver alloc] init];
	}
	return openEarsEventsObserver;
}

- (void) startListening {
    
    // startListeningWithLanguageModelAtPath:dictionaryAtPath:languageModelIsJSGF always needs to know the grammar file being used,
    // the dictionary file being used, and whether the grammar is a JSGF. You must put in the correct value for languageModelIsJSGF.
    // Inside of a single recognition loop, you can only use JSGF grammars or ARPA grammars, you can't switch between the two types.
    
    // An ARPA grammar is the kind with a .languagemodel or .DMP file, and a JSGF grammar is the kind with a .gram file.
    
    // If you wanted to just perform recognition on an isolated wav file for testing, you could do it as follows:
    
    // NSString *wavPath = [NSString stringWithFormat:@"%@/%@",[[NSBundle mainBundle] resourcePath], @"test.wav"];
    //[self.pocketsphinxController runRecognitionOnWavFileAtPath:wavPath usingLanguageModelAtPath:self.pathToGrammarToStartAppWith dictionaryAtPath:self.pathToDictionaryToStartAppWith languageModelIsJSGF:FALSE];  // Starts the recognition loop.
    
    // But under normal circumstances you'll probably want to do continuous recognition as follows:
    
    [self.pocketsphinxController startListeningWithLanguageModelAtPath:self.pathToDynamicallyGeneratedGrammar dictionaryAtPath:self.pathToDynamicallyGeneratedDictionary languageModelIsJSGF:FALSE];
}

- (void) stopListening {
    [self.pocketsphinxController stopListening];
}

- (void) initLanguageArray:(NSArray *)languageArray {
    //[OpenEarsLogging startOpenEarsLogging]; // Uncomment me for OpenEarsLogging
    
	[self.openEarsEventsObserver setDelegate:self]; // Make this class the delegate of OpenEarsObserver so we can get all of the messages about what OpenEars is doing.
    
    
    
    // This is the language model we're going to start up with. The only reason I'm making it a class property is that I reuse it a bunch of times in this example,
	// but you can pass the string contents directly to PocketsphinxController:startListeningWithLanguageModelAtPath:dictionaryAtPath:languageModelIsJSGF:
	
//	self.pathToGrammarToStartAppWith = [NSString stringWithFormat:@"%@/%@",[[NSBundle mainBundle] resourcePath], @"OpenEars1.languagemodel"];
    
	// This is the dictionary we're going to start up with. The only reason I'm making it a class property is that I reuse it a bunch of times in this example,
	// but you can pass the string contents directly to PocketsphinxController:startListeningWithLanguageModelAtPath:dictionaryAtPath:languageModelIsJSGF:
//	self.pathToDictionaryToStartAppWith = [NSString stringWithFormat:@"%@/%@",[[NSBundle mainBundle] resourcePath], @"OpenEars1.dic"];
    
//	self.usingStartLanguageModel = TRUE; // This is not an OpenEars thing, this is just so I can switch back and forth between the two models in this sample app.
	
	// Here is an example of dynamically creating an in-app grammar.
	
	// We want it to be able to response to the speech "CHANGE MODEL" and a few other things.  Items we want to have recognized as a whole phrase (like "CHANGE MODEL")
	// we put into the array as one string (e.g. "CHANGE MODEL" instead of "CHANGE" and "MODEL"). This increases the probability that they will be recognized as a phrase. This works even better starting with version 1.0 of OpenEars.
	
//	NSArray *languageArray = [[NSArray alloc] initWithArray:[NSArray arrayWithObjects: // All capital letters.
//                                                             @"LEFT",
//                                                             @"RIGHT",
//                                                             @"UP",
//                                                             @"DOWN",
//                                                             @"GRAB",
//                                                             @"DROP",
//                                                             //                                                             @"SUNDAY",
//                                                             //                                                             @"MONDAY",
//                                                             //                                                             @"TUESDAY",
//                                                             //                                                             @"WEDNESDAY",
//                                                             //                                                             @"THURSDAY",
//                                                             //                                                             @"FRIDAY",
//                                                             //                                                             @"SATURDAY",
//                                                             //                                                             @"QUIDNUNC",
//                                                             @"CHANGE MODEL",
//                                                             nil]];
//    
	// The last entry, quidnunc, is an example of a word which will not be found in the lookup dictionary and will be passed to the fallback method. The fallback method is slower,
	// so, for instance, creating a new language model from dictionary words will be pretty fast, but a model that has a lot of unusual names in it or invented/rare/recent-slang
	// words will be slower to generate. You can use this information to give your users good UI feedback about what the expectations for wait times should be.
    
	// I don't think it's beneficial to lazily instantiate LanguageModelGenerator because you only need to give it a single message and then release it.
	// If you need to create a very large model or any size of model that has many unusual words that have to make use of the fallback generation method,
	// you will want to run this on a background thread so you can give the user some UI feedback that the task is in progress.
    
	LanguageModelGenerator *languageModelGenerator = [[LanguageModelGenerator alloc] init];
    
    //    languageModelGenerator.verboseLanguageModelGenerator = TRUE; // Uncomment me for verbose debug output
    
    // generateLanguageModelFromArray:withFilesNamed returns an NSError which will either have a value of noErr if everything went fine or a specific error if it didn't.
	NSError *error = [languageModelGenerator generateLanguageModelFromArray:languageArray withFilesNamed:@"OpenEarsDynamicGrammar"];
    //    NSError *error = [languageModelGenerator generateLanguageModelFromTextFile:[NSString stringWithFormat:@"%@/%@",[[NSBundle mainBundle] resourcePath], @"OpenEarsCorpus.txt"] withFilesNamed:@"OpenEarsDynamicGrammar"]; // Try this out to see how generating a language model from a corpus works.
	NSDictionary *dynamicLanguageGenerationResultsDictionary = nil;
	if([error code] != noErr) {
		NSLog(@"Dynamic language generator reported error %@", [error description]);
	} else {
		dynamicLanguageGenerationResultsDictionary = [error userInfo];
		
		// A useful feature of the fact that generateLanguageModelFromArray:withFilesNamed: always returns an NSError is that when it returns noErr (meaning there was
		// no error, or an [NSError code] of zero), the NSError also contains a userInfo dictionary which contains the path locations of your new files.
		
		// What follows demonstrates how to get the paths for your created dynamic language models out of that userInfo dictionary.
		NSString *lmFile = [dynamicLanguageGenerationResultsDictionary objectForKey:@"LMFile"];
		NSString *dictionaryFile = [dynamicLanguageGenerationResultsDictionary objectForKey:@"DictionaryFile"];
		NSString *lmPath = [dynamicLanguageGenerationResultsDictionary objectForKey:@"LMPath"];
		NSString *dictionaryPath = [dynamicLanguageGenerationResultsDictionary objectForKey:@"DictionaryPath"];
		
		NSLog(@"Dynamic language generator completed successfully, you can find your new files %@\n and \n%@\n at the paths \n%@ \nand \n%@", lmFile,dictionaryFile,lmPath,dictionaryPath);
		
		// pathToDynamicallyGeneratedGrammar/Dictionary aren't OpenEars things, they are just the way I'm controlling being able to switch between the grammars in this sample app.
		self.pathToDynamicallyGeneratedGrammar = lmPath; // We'll set our new .languagemodel file to be the one to get switched to when the words "CHANGE MODEL" are recognized.
		self.pathToDynamicallyGeneratedDictionary = dictionaryPath; // We'll set our new dictionary to be the one to get switched to when the words "CHANGE MODEL" are recognized.
	}
	
	
    // Next, an informative message.
    
//	NSLog(@"\n\nWelcome to the OpenEars sample project. This project understands the words:\nBACKWARD,\nCHANGE,\nFORWARD,\nGO,\nLEFT,\nMODEL,\nRIGHT,\nTURN,\nand if you say \"CHANGE MODEL\" it will switch to its dynamically-generated model which understands the words:\nCHANGE,\nMODEL,\nMONDAY,\nTUESDAY,\nWEDNESDAY,\nTHURSDAY,\nFRIDAY,\nSATURDAY,\nSUNDAY,\nQUIDNUNC");
	
	// This is how to start the continuous listening loop of an available instance of PocketsphinxController. We won't do this if the language generation failed since it will be listening for a command to change over to the generated language.
	if(dynamicLanguageGenerationResultsDictionary) {
        
//        [self startListening];
        
	}
    
	// [self startDisplayingLevels] is not an OpenEars method, just an approach for level reading
	// that I've included with this sample app. My example implementation does make use of two OpenEars
	// methods:	the pocketsphinxInputLevel method of PocketsphinxController and the fliteOutputLevel
	// method of fliteController.
	//
	// The example is meant to show one way that you can read those levels continuously without locking the UI,
	// by using an NSTimer, but the OpenEars level-reading methods
	// themselves do not include multithreading code since I believe that you will want to design your own
	// code approaches for level display that are tightly-integrated with your interaction design and the
	// graphics API you choose.
	//
	// Please note that if you use my sample approach, you should pay attention to the way that the timer is always stopped in
	// dealloc. This should prevent you from having any difficulties with deallocating a class due to a running NSTimer process.
	
//	[self startDisplayingLevels];
}

// An optional delegate method of OpenEarsEventsObserver which delivers the text of speech that Pocketsphinx heard and analyzed, along with its accuracy score and utterance ID.
- (void) pocketsphinxDidReceiveHypothesis:(NSString *)hypothesis recognitionScore:(NSString *)recognitionScore utteranceID:(NSString *)utteranceID {
	NSLog(@"The received hypothesis is %@ with a score of %@ and an ID of %@", hypothesis, recognitionScore, utteranceID); // Log it.
	
//	self.heardTextView.text = [NSString stringWithFormat:@"Heard: \"%@\"", hypothesis]; // Show it in the status box.
	
	// This is how to use an available instance of FliteController. We're going to repeat back the command that we heard with the voice we've chosen.
//	[self.fliteController say:[NSString stringWithFormat:@"You said %@",hypothesis] withVoice:self.slt];
    
    UnitySendMessage("OpenEarsGameObject", "OnHeard", [hypothesis UTF8String]);
}

- (void) speak:(NSString *)phrase {
    
	[self.fliteController say:phrase withVoice:self.slt];
}

@end

UP_OpenEars *up_openEars = nil;

void UP_OpenEars_init(const char* ccsWords) {
    if (up_openEars)
        return;
    
    NSString *csWords = [NSString stringWithUTF8String:ccsWords];
    up_openEars = [[UP_OpenEars alloc] init];
    [up_openEars initLanguageArray:[csWords componentsSeparatedByString:@","]];
}

void UP_OpenEars_startListening() {
    if (!up_openEars)
        return;
    
    [up_openEars startListening];
}

void UP_OpenEars_stopListening() {
    if (!up_openEars)
        return;
    
    [up_openEars stopListening];
}

void UP_OpenEars_speak(const char* cphrase) {
    if (!up_openEars)
        return;
    
    NSString *phrase = [NSString stringWithUTF8String:cphrase];
    [up_openEars speak:phrase];
}

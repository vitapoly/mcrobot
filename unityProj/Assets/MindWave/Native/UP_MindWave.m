//
//  UP_OpenEars.m
//  Unity-iPhone
//
//  Created by Michael Wang on 7/13/13.
//
//

#import "UP_MindWave.h"


@implementation UP_MindWave


- (void)dataReceived:(NSDictionary *)data {
    //NSLog(@"Data received!");
    //NSLog(@"Raw: %d", [[data valueForKey:@"raw"] intValue]);
    //NSLog(@"Attention: %d", [[data valueForKey:@"eSenseAttention"] intValue]);
    
    [data retain];
    
    if([data valueForKey:@"blinkStrength"])
    {
        blinkStrength = [[data valueForKey:@"blinkStrength"] intValue];
        if ((blinkStrength != lastBlinkValue) && (blinkStrength > 90))
        {
            //NSLog(@"NEXT");
            lastBlinkValue = blinkStrength;
            directionValue++;
            if (directionValue > 3)
            {
                directionValue = 0;
            }
            
            NSString *tempString;
            switch (directionValue) {
                case 0:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "SET_LEFT");
                    tempString = @"LEFT";
                    break;
                case 1:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "SET_RIGHT");
                    tempString = @"RIGHT";
                    break;
                case 2:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "SET_OPEN");
                    tempString = @"OPEN";
                    break;
                case 3:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "SET_CLOSE");
                    tempString = @"CLOSE";
                    break;
            }
            
            NSLog(@"%d : %@", directionValue, tempString);
        }
    }
        
    if([data valueForKey:@"poorSignal"]) {
        poorSignalValue = [[data valueForKey:@"poorSignal"] intValue];
        if ((poorSignalValue == 0) && (prevSignalValue != 0)) {
                prevSignalValue = 0;
                //NSLog(@"No signal");
        }
        else if(poorSignalValue > 0 && poorSignalValue < 50) {
            if (prevSignalValue != 1)
            {
                prevSignalValue = 1;
                //NSLog(@"%d : +", poorSignalValue);
            }
        }
        else if(poorSignalValue > 50 && poorSignalValue < 200) {
            if (prevSignalValue != 2)
            {
                prevSignalValue = 2;
                //NSLog(@"%d : ++", poorSignalValue);
            }
        }
        else if(poorSignalValue == 200) {
            if (prevSignalValue != 3)
            {
                prevSignalValue = 3;
                //NSLog(@"%d : +++", poorSignalValue);
            }
        }
        else {
            if (prevSignalValue != 4)
            {
                prevSignalValue = 4;
                //NSLog(@"%d : GOOD SIGNAL", poorSignalValue);
            }
        }
    }
    
    // check to see whether the eSense values are there. if so, we assume that
    // all of the other data (aside from raw) is there. this is not necessarily
    // a safe assumption.
    if([data valueForKey:@"eSenseAttention"]){
        eSenseValues.attention =    [[data valueForKey:@"eSenseAttention"] intValue];
        eSenseValues.meditation =   [[data valueForKey:@"eSenseMeditation"] intValue];
        
        /*
         if ((eSenseValues.attention > 90) && (!concentrating))
         {
         concentrating = TRUE;
         NSLog(@"GO");
         } else if ((eSenseValues.attention <= 90) && (concentrating))
         {
         concentrating = FALSE;
         NSLog(@"STOP");
         }
         */
        if ((eSenseValues.meditation > 75) && (!relaxing))
        {
            relaxing = TRUE;
            NSLog(@"GO");
            
            // NOTE: **** This is where to send the command
            switch (directionValue) {
                case 0:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "LEFT");
                    NSLog(@"GO LEFT");
                    break;
                case 1:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "RIGHT");
                    NSLog(@"GO RIGHT");
                    break;
                case 2:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "OPEN");
                    NSLog(@"GO OPEN");
                    break;
                case 3:
                    UnitySendMessage("MindWaveGameObj", "OnSensed", "CLOSE");
                    NSLog(@"GO CLOSE");
                    break;
            }
        
        } else if ((eSenseValues.meditation <= 75) && (relaxing))
        {
            relaxing = FALSE;
            NSLog(@"STOP");
        }
        
    }
    
    // release the parameter
    [data release];
}

- (void)accessoryDidConnect:(EAAccessory *)accessory {
    NSLog(@"%@ was connected to this device.", [accessory name]);
}
- (void)accessoryDidDisconnect {
    NSLog(@"An accessory was disconnected.");
}

- (void)dealloc {
    // This may need to go somewhere else *****
    [[TGAccessoryManager sharedTGAccessoryManager] teardownManager];
    [super dealloc];
}


- (void) startListening {
     if([[TGAccessoryManager sharedTGAccessoryManager] accessory] != nil)
        [[TGAccessoryManager sharedTGAccessoryManager] startStream];
    directionValue = 0;
    NSLog(@"Now Listening");
    NSLog(@"LEFT");
    lastBlinkValue = 0;
    concentrating = FALSE;
    relaxing = FALSE;
    NSLog(@"STOP Mode");
    prevSignalValue = 0;
}

- (void) stopListening {
    if([[TGAccessoryManager sharedTGAccessoryManager] connected])
        [[TGAccessoryManager sharedTGAccessoryManager] stopStream];
}

@end

UP_MindWave *up_mindWave = nil;

void UP_MindWave_init() {
    if (up_mindWave)
        return;
    
    up_mindWave = [[UP_MindWave alloc] init];
    [[TGAccessoryManager sharedTGAccessoryManager] setupManagerWithInterval:0.05];
    [[TGAccessoryManager sharedTGAccessoryManager] setDelegate:up_mindWave] ;
}

void UP_MindWave_startListening() {
    if (!up_mindWave)
        return;
    
    [up_mindWave startListening];
}

void UP_MindWave_stopListening() {
    if (!up_mindWave)
        return;
    
    [up_mindWave stopListening];
}


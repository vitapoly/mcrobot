//
//  UP_OpenEars.h
//  Unity-iPhone
//
//  Created by Michael Wang on 7/13/13.
//
//

#import <Foundation/Foundation.h>

#import "TGAccessoryManager.h"
#import "TGAccessoryDelegate.h"
#import <ExternalAccessory/ExternalAccessory.h>

// the eSense values
typedef struct {
    int attention;
    int meditation;
} ESenseValues;


@interface UP_MindWave : NSObject<TGAccessoryDelegate> {
    int directionValue; // 0=LEFT, 1=UP, 2=RIGHT, 3=DOWN, 4=PICK, 5=DROP
    int lastBlinkValue;
    BOOL concentrating;
    BOOL relaxing;    

    int blinkStrength;
    int poorSignalValue;
    int prevSignalValue;
    ESenseValues eSenseValues;

}

// TGAccessoryDelegate protocol methods
- (void)accessoryDidConnect:(EAAccessory *)accessory;
- (void)accessoryDidDisconnect;
- (void)dataReceived:(NSDictionary *)data;


@end

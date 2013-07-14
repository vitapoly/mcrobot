//
//  AppDelegate.h
//  mcrobot
//
//  Created by Mark Chen on 7/13/13.
//  Copyright (c) 2013 iosdevcamp. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <PushIOManager/PushIOManager.h>

@interface AppDelegate : UIResponder <UIApplicationDelegate, PushIOManagerDelegate>

@property (strong, nonatomic) UIWindow *window;

@end

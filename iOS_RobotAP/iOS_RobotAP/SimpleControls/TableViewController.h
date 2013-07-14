//
//  TableViewController.h
//  SimpleControl
//
//  Created by Cheong on 7/11/12.
//  Copyright (c) 2012 RedBearLab. All rights reserved.
//
//  modify by Powen Ko on 7/13/2013

#import <UIKit/UIKit.h>
#import "BLE.h"

@interface TableViewController : UITableViewController <BLEDelegate>
{
    IBOutlet UIButton *btnConnect;
    IBOutlet UISwitch *swDigitalIn;
    IBOutlet UISwitch *swDigitalOut;
    IBOutlet UISwitch *swAnalogIn;
    IBOutlet UILabel *lblAnalogIn;
    IBOutlet UISlider *sldPWM;
    IBOutlet UISlider *sldServo;
    IBOutlet UISlider *sldServo1;
    IBOutlet UISlider *sldServo2;
    IBOutlet UISlider *sldServo3;
    IBOutlet UIActivityIndicatorView *indConnecting;
    IBOutlet UILabel *lblRSSI;
}

@property (strong, nonatomic) BLE *ble;

@end

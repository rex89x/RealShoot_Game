#include <Wire.h>

#define DEVICE (0x53)
#define TO_READ (6)

#include <SparkFun_ADXL345.h>

ADXL345 __flagADXL345 = ADXL345();

int __ADXL345xyz[3];

int __getAccel(int axis) {
  __flagADXL345.readAccel(__ADXL345xyz);
  return (__ADXL345xyz[axis]);
}

String __rightPaddingStr(String content, int width) {
  int len = content.length();
  for(int i = 0;i < (width - len);i++)
    content += " ";
  return content;
}

byte buff[TO_READ];
char str[512];
int regAddress = 0x32;
int x, y, z;
double roll = 0.00, pitch = 0.00;


void setup() {
  Wire.begin(); 

  __flagADXL345.powerOn();
  __flagADXL345.setRangeSetting(16);
  __flagADXL345.setFullResBit(true);
  
  Serial.begin(9600);

  writeTo(DEVICE, 0x2D, 0);      
  writeTo(DEVICE, 0x2D, 16);
  writeTo(DEVICE, 0x2D, 8);

}

void loop() {

  readFrom(DEVICE, regAddress, TO_READ, buff);
  x = (((int)buff[1]) << 8) | buff[0];   
  y = (((int)buff[3])<< 8) | buff[2];
  z = (((int)buff[5]) << 8) | buff[4];

  Serial.print("The acceleration info of x, y, z are:");
  sprintf(str, "%d %d %d", x, y, z);  
  Serial.print(str);
  Serial.write(10);

  RP_calculate();
  Serial.print("Roll:"); Serial.println( roll ); 
  Serial.print("Pitch:"); Serial.println( pitch );
  Serial.println("");
  Serial.println((String(__getAccel(0)) + String(u8",") + String(__getAccel(1)) + String(u8",") + String(__getAccel(2))));
  delay(800);
}


void writeTo(int device, byte address, byte val) {
  Wire.beginTransmission(device);
  Wire.write(address);
  Wire.write(val);
  Wire.endTransmission();
}


void readFrom(int device, byte address, int num, byte buff[]) {
  Wire.beginTransmission(device);
  Wire.write(address);
  Wire.endTransmission();

    Wire.beginTransmission(device);
  Wire.requestFrom(device, num);

  int i = 0;
  while(Wire.available())
  { 
    buff[i] = Wire.read();
    i++;
  }
  Wire.endTransmission();
}

void RP_calculate(){
  double x_Buff = float(x);
  double y_Buff = float(y);
  double z_Buff = float(z);
  roll = atan2(y_Buff , z_Buff) * 57.3;
  pitch = atan2((- x_Buff) , sqrt(y_Buff * y_Buff + z_Buff * z_Buff)) * 57.3;
}

#include <math.h>
#include <Ethernet.h>
#include <SPI.h>

int pin= A2;
double VOLT=5;
double v;/*voltaggio tra 0 e 1023*/
double volt;/* voltaggio reale tra o e 5 volt*/
double Rt;/* resistenza del termistore*/
double R1=10000;/*resistenza in ohm che abbiamo messo noi*/
double temp;/* temperatura in Kelvin*/
double Temperatura;
double Umidita;
EthernetClient client;


byte mac[] = {  0x90, 0xA2, 0xDA, 0x0D, 0xF6, 0xFF }; 
byte ip[] = {  192, 168, 1, 180};

void setup(){

  Serial.begin(9600);/*usiamo il display seriale*/
  Ethernet.begin(mac, ip);
  
  Serial.print("IP Address        : ");

  Serial.println(Ethernet.localIP());

  Serial.print("Subnet Mask       : ");

  Serial.println(Ethernet.subnetMask());

  Serial.print("Default Gateway IP: ");

  Serial.println(Ethernet.gatewayIP());

  Serial.print("DNS Server IP     : ");

  Serial.println(Ethernet.dnsServerIP());

if(client.connect("192.168.1.196",80)){Serial.println("Connesso");}
}

void loop(){

  
 /*v=analogRead(pin);//leggiamo il valore della tensione ai capi di R1
  volt=5*v/1023;// riportiamo la tensine tra 0 e 5 volt con una proporzione
  Rt=R1*(5/volt-1);//resistenza del termistore
  temp=1/(0.001319+(0.000234125*log(Rt))+(0.0000000876741*log(Rt)*log(Rt)*log(Rt)));//calcolo la temperatura con la formula di Steinhart-Hart
  Temperatura=temp-273.15;// gradi Chelsius
  */
  delay(1000);/*ritardo di un secondo*/  
  
  String ins = "GET http://192.168.1.196/arduino.php?temperatura=190&umidita=145  HTTP/1.1";
  
  //Serial.println(ins);
    
  //Invio Dati al server
  if (client.connected()) {
    Serial.println("OK");
    client.println(ins);
    client.println("Host: 192.168.1.196");
    client.println("User-Agent: Arduino-Ethernet");
    client.println("Connection: close");
    client.println();
    if (client.available()) {
      for(int i=0; i<100; i++)
      Serial.write(client.read());
    }
  }
}

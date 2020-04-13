#include <iostream>
#include <fstream>
#include <vector>

using namespace std;

unsigned char reverseBits(unsigned char data) {
   for (int data = 0; data < 255; data++) {  //��� 255 - ���������� ������. � ������ � ������� �� 1 �� - ��� 1*10^9 ����
   data = (data & 0x55) << 1 | (data & 0xAA) >> 1;
   data = (data & 0x33) << 2 | (data & 0xCC) >> 2;
   data = (data & 0x0F) << 4 | (data & 0xF0) >> 4;
   }
   return data;
}

unsigned char inverseBits(unsigned char date) {
for (int date = 0; date < 255; date++) { //�� ��������
date |= date >> 1;
date |= date >> 2;
date |= date >> 4;
date |= date >> 8;
date |= date >> 16;
int inverseBit = ~date;
}
return date;
}

int main()
{
    int FILE_CHOOSE;
    unsigned char data;
    unsigned char date;
    ifstream infile;
    infile.open("B:\\bits.txt");
    infile >> data;
    cout << "Type 1 for reverse, 2 for inverse" << endl;
    cin >> FILE_CHOOSE ;

     switch (FILE_CHOOSE)
     {
            case 1:
     {
     ofstream out ("B:\\reversed.txt");
     out << (reverseBits(data))<< endl;
     infile.close();
  break;
     }
     //break;
            case 2:
     {
     ofstream inv ("B:\\inversed.txt"); // ������ ����������
     inv << (inverseBits(date))<< endl;
     infile.close();
break;
     }
     //break;
            default:
                break;

}
}

**Devrenin Çalışma Prensibi**

  Genel mesaj yapısı şu elemanlardan oluşur

**1.Byte ** = Prefix. Mesajın başladığı anlamına gelen Byte'dır. 0x48 olarak tanımlanmıştır.

**2.Byte** = Header Byte'ıdır. Bu mesaj yapısına göre değer alır. Mesajın işlevini belirtir.

**3.Byte** = Mesaj Uzunluğu Byte'ıdır. Toplam mesaj uzunluğunu değil data uzunluğunu belirtir. Aşşağıda n olarak belirtilen uzunluk mesaj uzunluğudur.

**4-n.Byte** = Data byte'larıdır. Burada gönderilecek mesajlar sıralanır.

**n+1.Byte** = BCC byte'ıdır. Burada 1.Byte'tan itibaren tüm byte'lar xor işlemine tabi tutulur. 

**n+2.Byte** = Mesajın bittiği anlamına gelen byte'dır. 0x0A olarak tanımlanmıştır.


  C# uygulaması 100ms de bir sorgu gönderir. STM32 bu sorguya yanıt gönderir.
Sorgu içerisinde trackbardan gelen dac değeri gönderilir. 
STM32 Sorguya ADC değerlerini , Hesaplanan voltaj değerini , İşlemcinin Gain ve Ofset değerlerini içeren bir yanıt mesajı gönderir.

  C# uygulamasından ofset ve voltaj değerleri girildiğinde, iki seçenek vardır. Update value ve Save value.
Update value, gain ve offset değerini değiştir ama Flash memory'e kaydetmez. STM32 yeniden başlatıldığında Flash memoryde olan değerler ile başlatılır.
Save value, gain ve offset değerlerini Flash memory'e kaydeder. STM32 yeniden başlatıldığında bu değerler ile başlatılır.

STM32F4 Discovery kartı ile iki kanal ADC okuma ve seri port üzerinden C# Windows form uygulaması ile uart üzerinden haberleşme uygulaması. Voltaj hesaplaması, ofset, gain ve kayan ortalama yöntemi kullanılmıştır. 

**STM32 bağlantıları resimde görüldüğü gibidir.**
![image](https://github.com/user-attachments/assets/ca18a899-9828-40be-bd27-14b436a5bdba)

**Breadboard üzerinde kurduğum devre resimde gösterilmiştir.**

![image](https://github.com/user-attachments/assets/ca8e7236-47e2-495d-a462-3938007234d1)


**C# ile yaptığım devre resimde görüldüğü gibidir.**

![image](https://github.com/user-attachments/assets/8a0e16e1-6359-4b38-9c35-dd071735a5f1)

**Osiloskopta DAC değerinin görüntüsü resimdeki şekildedir.**
![WhatsApp Görsel 2025-07-06 saat 22 49 30_61c795b4](https://github.com/user-attachments/assets/7c091288-92de-49a4-8a49-5d3a90c02615)

**ADC için giriş voltaj değerinin görüntüsü resimdeki şekildedir.**
![WhatsApp Görsel 2025-07-06 saat 22 49 29_9b5950d3](https://github.com/user-attachments/assets/f62ecf27-5367-4962-8834-63812c82f6ce)



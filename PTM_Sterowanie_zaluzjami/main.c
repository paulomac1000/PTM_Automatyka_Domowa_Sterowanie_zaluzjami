#include "stm32f4xx_conf.h"
#include "stm32f4xx_gpio.h"
#include "stm32f4xx_rcc.h"
#include "stm32f4xx_tim.h"
#include "stm32f4xx_exti.h"
#include "misc.h"
#include "string.h"
#include "stdio.h"
#include "stm32f4xx_syscfg.h"
#include "stm32f4xx_usart.h"
#include "stdlib.h"

volatile uint8_t zmienna;
volatile char odebrane[25];
volatile char komunikat[13];
int czy_czujnik;

void init_tim3()
{
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3, ENABLE);
	TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;

	TIM_TimeBaseStructure.TIM_Period = 8399;
	TIM_TimeBaseStructure.TIM_Prescaler = (9999*2);
	TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
	TIM_TimeBaseStructure.TIM_CounterMode =  TIM_CounterMode_Up;
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);

}
void led()
{
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOD, ENABLE);

	GPIO_InitTypeDef GPIO_InitStructure;

	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_12 | GPIO_Pin_13| GPIO_Pin_14| GPIO_Pin_15;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_NOPULL;
	GPIO_Init(GPIOD, &GPIO_InitStructure);
}


void init_usart()
{
	// wlaczenie taktowania wybranego portu
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOC,ENABLE);

	// konfiguracja linii Tx
	GPIO_PinAFConfig(GPIOC, GPIO_PinSource10, GPIO_AF_USART3);
	GPIO_InitTypeDef GPIO_InitStructure;
	GPIO_InitStructure.GPIO_OType=GPIO_OType_PP;
	GPIO_InitStructure.GPIO_PuPd=GPIO_PuPd_UP;
	GPIO_InitStructure.GPIO_Mode=GPIO_Mode_AF;
	GPIO_InitStructure.GPIO_Pin=GPIO_Pin_10;

	GPIO_InitStructure.GPIO_Speed=GPIO_Speed_50MHz;

	GPIO_Init(GPIOC, &GPIO_InitStructure);
	// konfiguracja linii Rx
	GPIO_PinAFConfig(GPIOC, GPIO_PinSource11, GPIO_AF_USART3);
	GPIO_InitStructure.GPIO_Mode=GPIO_Mode_AF;
	GPIO_InitStructure.GPIO_Pin	= GPIO_Pin_11;

	GPIO_Init(GPIOC, &GPIO_InitStructure);

	// wlaczenie taktowania wybranego uk ³ adu USART
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3,ENABLE);

	USART_InitTypeDef USART_InitStructure;
	// predkosc transmisji (mozliwe standardowe opcje: 9600, 19200, 38400, 57600,115200, ...)
	USART_InitStructure.USART_BaudRate= 9600;
	// d ³ ugo œæ s ³ owa (USART_WordLength_8b lub USART_WordLength_9b)
	USART_InitStructure.USART_WordLength= USART_WordLength_8b;
	// liczba bitów stopu (USART_StopBits_1, USART_StopBits_0_5,USART_StopBits_2, USART_StopBits_1_5)
	USART_InitStructure.USART_StopBits= USART_StopBits_1;
	// sprawdzanie parzysto œ ci (USART_Parity_No, USART_Parity_Even,USART_Parity_Odd)
	USART_InitStructure.USART_Parity= USART_Parity_No;
	// sprz ê towa kontrola przep ³ ywu (USART_HardwareFlowControl_None,USART_HardwareFlowControl_RTS, USART_HardwareFlowControl_CTS,USART_HardwareFlowControl_RTS_CTS)
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
	// tryb nadawania/odbierania (USART_Mode_Rx, USART_Mode_Rx )
	USART_InitStructure.USART_Mode= USART_Mode_Rx | USART_Mode_Tx;
	// konfiguracja
	USART_Init(USART3, &USART_InitStructure);

	// wlaczenie ukladu USART
	USART_Cmd(USART3,ENABLE);
}

void init_usart_przerwania()
{
	NVIC_InitTypeDef NVIC_InitStructure;

	// wlaczenie przerwania zwi ¹ zanego z odebraniem danych (pozostale zrodlaprzerwan zdefiniowane sa w pliku stm32f4xx_usart.h)
	USART_ITConfig(USART3, USART_IT_RXNE,ENABLE);
	NVIC_InitStructure.NVIC_IRQChannel=USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority= 0;
	NVIC_InitStructure.NVIC_IRQChannelSubPriority= 0;
	NVIC_InitStructure.NVIC_IRQChannelCmd= ENABLE;
	// konfiguracja kontrolera przerwan
	NVIC_Init(&NVIC_InitStructure);
	// wlaczenie przerwan od ukladu USART
	NVIC_EnableIRQ(USART3_IRQn);
}

void USART3_IRQHandler(void)
{
	// sprawdzenie flagi zwiazanej z odebraniem danych przez USART

	if(USART_GetITStatus(USART3, USART_IT_RXNE) != RESET)
	{
		while(USART_GetFlagStatus(USART3, USART_FLAG_RXNE) == RESET);
		// odebrany bajt znajduje sie w rejestrze USART3->DR
		zmienna = (uint16_t)USART3->DR;

		char bufor[1];
		bufor[0] = (char)zmienna;
		strncat(odebrane, bufor, 1);
	}
}

void send_char(char znak)
{
	//czekaj na opró ¿ nienie bufora wyj œ ciowego
	while(USART_GetFlagStatus(USART3, USART_FLAG_TXE) == RESET);
	// wyslanie danych
	USART_SendData(USART3,znak);
	// czekaj az dane zostana wyslane
	while(USART_GetFlagStatus(USART3, USART_FLAG_TC) == RESET);
}
void send_error()
{
	send_string("Error!");
}

void send_int(int liczba)
{
	char bufor[5];

	itoa(liczba, bufor, 10);

	send_string(bufor);
}

void send_string(const char *bufor)
{
	while(*bufor)
	{
		send_char(*bufor++);
	}
}

void pobierz_komunikat()
{
	int i = 1; //od pozycji nr 1 zaczyna siê treæ komunikatu

    while(odebrane[i] != 30)
    {
    	komunikat[i-1] = odebrane[i];
    	i++;
    }
}

int pobierz_sume_kontrolna()
{
	int i = 1; //od pozycji nr 1 zaczyna siê tresæ komunikatu

	int suma_kontrolna = 0;
	while(odebrane[i] != 30)
	{
		i++;
	}

	i++;

    while(odebrane[i] != 3)
    {
    	suma_kontrolna+=odebrane[i];
    	i++;
    }

    return suma_kontrolna;
}

int oblicz_sume_kontrolna()
{
	int i = 0;
	int x = 0;

    while(komunikat[i] != 0)
    {
    	x += (int)komunikat[i];
    	i++;
    }
    return x;
}

void zeruj_odebrane()
{
	int i = 0;

	for (i=0; i<25; i++)
	{
		odebrane[i] = (char)0;
	}
}

void zeruj_komunikat()
{
	int i = 0;

	for (i=0; i<13; i++)
	{
		komunikat[i] = (char)0;
	}
}

void wyodrebnij_komunikat()
{
	// budowa pakietu
	// STX XXXXXXXXXXXXX RS xxxx ETX
	// STX - pocz¹tek pakietu, tablica ASCII nr 2
	// XXX komunikat, 1-13 znaków
	// RS - separator treœci komunikatu i sumy kontrolnej, tablica ASCII nr 30
	// xxxx suma kontrolna, bêd¹ca sum¹ numerów z tablicy ASCII znaków treœci komunikatu, 1-4 znaków
	// ETX - koniec pakietu, tablica ASCII nr 3
	// dlugosc to max 20 znakow

    //sprawdzam, czy pakiet nie jest za d³ugi
    if (strlen(odebrane) > 20)
    {
    	send_error();
    	send_string("Za dlugi pakiet!");
    	zeruj_odebrane();
    }
    else
    {
        // sprawdzam, czy zosta³ w ca³oœci przes³any pakiet (czy siê koñczy - ma znak ETX)
        if (strchr(odebrane,3))
        {
            // teraz sprawdzam, czy pakiet zawiera elementy STX i RS
            if(strchr(odebrane,2) && strchr(odebrane,30))
            {
            	pobierz_komunikat();

            	int suma_kontrolna = pobierz_sume_kontrolna();
            	//send_string(komunikat);
            	if(suma_kontrolna == oblicz_sume_kontrolna())
            	{
            		send_string(" Suma kontrolna zgodna dla komendy: ");
            		send_string(komunikat);
            		send_char('!');

            		//send_string(komunikat);
            		zeruj_odebrane();
            		return;
            	}
            	else
            	{
        			send_error();
            		send_string("suma kontrolna nie zgodna!");
            		zeruj_komunikat();
            	}
            }
            else
            {
                // je¿eli pakiet nie zawiera elementów STX i/lub RS
            	send_error();
            	send_string(" Bledny format komunikatu: ");
            	send_string(odebrane);
            	send_char('!');
            }
        	zeruj_odebrane();
        	return;
        }
        return;
    }
}

void porty_silnik()
{
	/* GPIOD Periph clock enable */
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOB, ENABLE);

	GPIO_InitTypeDef gpio;  // obiekt gpio bêd¹cy konfiguracj¹ portów GPIO

	GPIO_StructInit(&gpio);  // domyœlna konfiguracja
	gpio.GPIO_Pin = GPIO_Pin_5 | GPIO_Pin_6;  // konfigurujemy pin 5
	gpio.GPIO_Mode = GPIO_Mode_OUT;  // jako wyjœcie
	gpio.GPIO_OType = GPIO_OType_PP;
	gpio.GPIO_Speed = GPIO_Speed_100MHz;
	gpio.GPIO_PuPd =  GPIO_PuPd_UP;
	GPIO_Init(GPIOB, &gpio);  // inicjalizacja modu³u GPIOA

	GPIO_SetBits(GPIOB, GPIO_Pin_5 | GPIO_Pin_6); //wartosc pocz¹tkowa
}

void porty_czujnik_swiatla()
{
	/* GPIOD Periph clock enable */
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOA, ENABLE);

	GPIO_InitTypeDef gpio;  // obiekt gpio bêd¹cy konfiguracj¹ portów GPIO

	GPIO_StructInit(&gpio);  // domyœlna konfiguracja
	gpio.GPIO_Pin = GPIO_Pin_1;  // konfigurujemy pin 1
	gpio.GPIO_Mode = GPIO_Mode_IN;  // jako wejœcie
	gpio.GPIO_Speed = GPIO_Speed_100MHz;
	gpio.GPIO_PuPd =  GPIO_PuPd_NOPULL;
	GPIO_Init(GPIOA, &gpio);  // inicjalizacja modu³u GPIOA
}
enum Stan_rolety
{
	stop,
	rozwijana,
	zwijana,
	rozwinieta,
	zwinieta
};
int main(void)
{
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3, ENABLE);
	TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;

	TIM_TimeBaseStructure.TIM_Period = 8399;
	TIM_TimeBaseStructure.TIM_Prescaler = (9999*2);
	TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
	TIM_TimeBaseStructure.TIM_CounterMode =  TIM_CounterMode_Up;
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	TIM_ClearFlag(TIM3, TIM_FLAG_Update);

	enum Stan_rolety stan_rolety = zwinieta;
	czy_czujnik = 0;
	int licznik = 0;
	int ile_bylo_swiatla = 0;
	int period = 8399;

	led();
	GPIO_SetBits(GPIOD, GPIO_Pin_15 | GPIO_Pin_14); //dwie zapalone diody (nieb i czerw) informuj¹ ¿e tryb jest auto
	//init_tim3();

	init_usart();
	init_usart_przerwania();

	porty_silnik();
	porty_czujnik_swiatla();

	send_string("Hello World!");
	zeruj_odebrane();

	while(1)
	{
		if(TIM_GetFlagStatus(TIM3, TIM_FLAG_Update))
			{
				if(czy_czujnik==0)
				{
					GPIO_ResetBits(GPIOD, GPIO_Pin_15);
					GPIO_ResetBits(GPIOD, GPIO_Pin_14);
				}
				GPIO_ResetBits(GPIOB, GPIO_Pin_5);
				GPIO_ResetBits(GPIOB, GPIO_Pin_6);
				TIM_Cmd(TIM3, DISABLE);
				period = TIM_TimeBaseStructure.TIM_Period;
				if (period != 8399)
				{
					TIM_TimeBaseStructure.TIM_Period = 8399;
					TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
				}
				if(stan_rolety==rozwijana)
				{
					stan_rolety=rozwinieta;
					send_string("Roleta rozwinieta!");
				}
				else if(stan_rolety==zwijana)
				{
					stan_rolety=zwinieta;
					send_string("Roleta zwinieta!");
				}
				TIM_ClearFlag(TIM3, TIM_FLAG_Update);
			}

		if(czy_czujnik == 1)
		{
			if(licznik<100000)
			{
				licznik++;
			}
			if(!GPIO_ReadInputDataBit(GPIOA, GPIO_Pin_1))
			{
				ile_bylo_swiatla++;
				//send_string("ODBIERAM swiatlo!");
				if(licznik == 100000)
				{
					send_string("Licznik pelny!");
					licznik=0;
					if(((double)(ile_bylo_swiatla/100000.0))>=0.95)
					{		if(stan_rolety==zwinieta)
							{
								send_string("Roleta zwinieta!");
								licznik=0;
							}
							else
							{
								GPIO_SetBits(GPIOD,GPIO_Pin_12);
								ile_bylo_swiatla = 0;
								send_string("Swiatlo padalo wystarczajaco!");
								send_string("Zwijam rolete!");
								stan_rolety=zwijana;
								TIM_Cmd(TIM3, DISABLE);
								GPIO_ResetBits(GPIOB, GPIO_Pin_6);
								GPIO_SetBits(GPIOB, GPIO_Pin_5);
								TIM_Cmd(TIM3, ENABLE);
							}

					}
					else
					{
							GPIO_ResetBits(GPIOD,GPIO_Pin_12);
							send_string("Przekroczono margines bledu!");
							send_string("Nie ruszam rolety!");
					}
				}
			}
			else
			{
				if(licznik==100000)
				{
					if(stan_rolety==rozwinieta)
					{
						send_string("Roleta rozwinieta!");
						licznik=0;
					}
					else
					{
						TIM_Cmd(TIM3, DISABLE);
						licznik=0;
						send_string("Brak swiatla!");
						send_string("Rozwijam rolete!");
						stan_rolety = rozwijana;
						GPIO_ResetBits(GPIOB, GPIO_Pin_5);
						GPIO_SetBits(GPIOB, GPIO_Pin_6);
						TIM_Cmd(TIM3, ENABLE);
					}
				}
			}
		}
		GPIO_ResetBits(GPIOD,GPIO_Pin_12);

		wyodrebnij_komunikat();

		if(strcmp(komunikat, "rozwin")== 0)
		{
			TIM_Cmd(TIM3, DISABLE);
			czy_czujnik = 0;

			if(stan_rolety==rozwinieta)
			{
				send_string("Roleta rozwinieta!");
			}
			else
			{
				stan_rolety = rozwijana;

				period = TIM_TimeBaseStructure.TIM_Period;
				send_string("Period: ");
				send_int(period);
				send_char('!');

				if(GPIO_ReadInputDataBit(GPIOB, GPIO_Pin_5) == 1
					|| GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_15) == 1)
				{
					GPIO_ResetBits(GPIOD, GPIO_Pin_15);
					GPIO_ResetBits(GPIOB, GPIO_Pin_5);

				}
					GPIO_SetBits(GPIOD,GPIO_Pin_14);
				GPIO_SetBits(GPIOB, GPIO_Pin_6);
				send_string("Zapalam diode 14!");
				send_string("Rozwijam rolete!");
				TIM_Cmd(TIM3, ENABLE);
			}
			zeruj_komunikat();
		}
		else if(strcmp(komunikat, "zwin")== 0)
		{
			TIM_Cmd(TIM3, DISABLE);
			czy_czujnik = 0;

			if(stan_rolety == zwinieta)
			{
				send_string("Roleta zwinieta!");
			}
			else
			{
				stan_rolety=zwijana;
				int period1 = TIM_TimeBaseStructure.TIM_Period;
				send_string("Period: ");
				send_int(period1);
				send_char('!');
				period *=1.1;
				TIM_TimeBaseStructure.TIM_Period = period;
				TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
				TIM_ClearFlag(TIM3, TIM_FLAG_Update);

				if(GPIO_ReadInputDataBit(GPIOB, GPIO_Pin_6) == 1
						|| GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_14) == 1)
				{
					GPIO_ResetBits(GPIOD, GPIO_Pin_14);
					GPIO_ResetBits(GPIOB, GPIO_Pin_6);
				}
				GPIO_SetBits(GPIOD,GPIO_Pin_15);
				GPIO_SetBits(GPIOB, GPIO_Pin_5);
				send_string("Zapalam diode 15!");
				send_string("Zwijam rolete!");
				TIM_Cmd(TIM3, ENABLE);
			}

			zeruj_komunikat();
		}
		else if(strcmp(komunikat, "auto")==0)
		{
			czy_czujnik = 1;
			GPIO_SetBits(GPIOD,GPIO_Pin_15 | GPIO_Pin_14);
			send_string("Przechodze w tryb automatyczny!");
			send_string("Zapalam diody 14 i 15!");
			//TIM_Cmd(TIM3, ENABLE);
			zeruj_komunikat();
		}
		else if(strcmp(komunikat, "stop")==0)
		{
			TIM_Cmd(TIM3, DISABLE);
			czy_czujnik = 0;
			int x = TIM3->CNT;
			send_string("Timer doliczyl do: ");
			send_int(x);
			send_char('!');
			if(x>0)
			{
				TIM_TimeBaseStructure.TIM_Period = x;
				TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
				TIM_ClearFlag(TIM3, TIM_FLAG_Update);
			}

			period = TIM_TimeBaseStructure.TIM_Period;
			send_string("Period po zmianie: ");
			send_int(period);
			send_char('!');

			GPIO_SetBits(GPIOB, GPIO_Pin_5 | GPIO_Pin_6);
			GPIO_ResetBits(GPIOD, GPIO_Pin_15);
			GPIO_ResetBits(GPIOD, GPIO_Pin_14);
			send_string("Gasze diody 14 i 15!");
			send_string("Zatrzymuje rolete!");
			zeruj_komunikat();
		}
		else
		{
			if(strcmp(komunikat,"")!=0)
			{
				send_error();
				zeruj_komunikat();
			}
		}
	}
}

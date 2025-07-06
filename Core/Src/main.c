/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * Copyright (c) 2025 STMicroelectronics.
  * All rights reserved.
  *
  * This software is licensed under terms that can be found in the LICENSE file
  * in the root directory of this software component.
  * If no LICENSE file comes with this software, it is provided AS-IS.
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "main.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "string.h"

volatile uint8_t timer_100uS=0;
uint8_t timer_1mS=0,timer_100mS=0,timer_1Sec=0,timer_3Sec=0,timer_10Sec=0;
uint32_t adc_raw=0;
uint16_t DAC_Val=0;
uint32_t VoltageVAl=0,Voltage_Filt=0;
uint8_t ReceiveData=0;
float V_meas=0;
float V_real=0;
uint16_t rx_index = 0;
uint8_t bcc_rx=0;
uint8_t rx_buffer[50];
uint8_t data_length=0;
uint8_t data_index=0;
float new_offset=0;
float new_gain=0;

uint16_t ADC_VAL[2];
volatile uint16_t adc_ch0 = 0;
volatile uint16_t adc_ch1 = 0;


typedef struct
{
    float adc_offset;
    float gain_correction;
    //parametre eklenirse
    //@add_param
    uint32_t valid_flag; // sonradan kontrol için
} CalibrationData_t;

CalibrationData_t calib_data;
/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */

/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */
#define FLASH_USER_START_ADDR  ((uint32_t)0x080E0000) // Sector 11 start
#define FLASH_USER_SECTOR      FLASH_SECTOR_11
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */
void send_voltage_packet(uint32_t raw_data,float real_data, float offset, float gain);
void calculate_adc_val(void);
void SaveCalibrationData(const CalibrationData_t *data);
void LoadCalibrationData(CalibrationData_t *data);
void Reset_Rx_Buffer(void);
void procces_rx_message(uint8_t *data);
/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/
ADC_HandleTypeDef hadc1;
DMA_HandleTypeDef hdma_adc1;

DAC_HandleTypeDef hdac;

TIM_HandleTypeDef htim1;
TIM_HandleTypeDef htim2;

UART_HandleTypeDef huart4;

/* USER CODE BEGIN PV */

/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_DMA_Init(void);
static void MX_ADC1_Init(void);
static void MX_TIM1_Init(void);
static void MX_TIM2_Init(void);
static void MX_DAC_Init(void);
static void MX_UART4_Init(void);
/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */

/* USER CODE END 0 */

/**
  * @brief  The application entry point.
  * @retval int
  */
int main(void)
{
  /* USER CODE BEGIN 1 */

  /* USER CODE END 1 */

  /* MCU Configuration--------------------------------------------------------*/

  /* Reset of all peripherals, Initializes the Flash interface and the Systick. */
  HAL_Init();

  /* USER CODE BEGIN Init */

  /* USER CODE END Init */

  /* Configure the system clock */
  SystemClock_Config();

  /* USER CODE BEGIN SysInit */

  /* USER CODE END SysInit */

  /* Initialize all configured peripherals */
  MX_GPIO_Init();
  MX_DMA_Init();
  MX_ADC1_Init();
  MX_TIM1_Init();
  MX_TIM2_Init();
  MX_DAC_Init();
  MX_UART4_Init();
  /* USER CODE BEGIN 2 */
  //HAL_UART_Transmit(&huart1, 0, 1, 10);
  //HAL_UART_Receive_IT(&huart4, &ReceiveData, 1);
  UART4->CR1 |= USART_CR1_RXNEIE;


  TIM2->DIER |= TIM_DIER_UIE;
  TIM2->CR1 |= TIM_CR1_CEN;
  LoadCalibrationData(&calib_data);
  HAL_DAC_Start(&hdac, DAC_CHANNEL_1);
  HAL_ADC_Start_DMA(&hadc1, (uint32_t*)ADC_VAL, 2);
  /* USER CODE END 2 */

  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
	  if(timer_100uS>9)//100uS Loop
	  {timer_1mS++;
		  calculate_adc_val();
		  HAL_DAC_SetValue(&hdac, DAC_CHANNEL_1,DAC_ALIGN_12B_R, DAC_Val);
		  timer_100uS=0;
	  }
	  if(timer_1mS>9)//1mS Loop
	  {timer_100mS++;
	  GPIOD->ODR ^= (1<<13);
		  timer_1mS=0;
	  }
	  if(timer_100mS>99)//100mS Loop
	  {timer_1Sec++;
	  GPIOD->ODR ^= (1<<14);
		  timer_100mS=0;
	  }
	  if(timer_1Sec>9)//1Sec Loop
	  {timer_10Sec++;timer_3Sec++;
	  GPIOD->ODR ^= (1<<15);
		  timer_1Sec=0;
	  }
	  if(timer_3Sec>2)//3Sec Loop
	  {
		  GPIOD->ODR ^= (1<<12);
		  timer_3Sec=0;
	  }
	  if(timer_10Sec>9)//10Sec Loop
	  {
		  GPIOD->ODR ^= (1<<10);
		  timer_10Sec=0;
	  }
    /* USER CODE END WHILE */

    /* USER CODE BEGIN 3 */
  }
  /* USER CODE END 3 */
}

/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

  /** Configure the main internal regulator output voltage
  */
  __HAL_RCC_PWR_CLK_ENABLE();
  __HAL_PWR_VOLTAGESCALING_CONFIG(PWR_REGULATOR_VOLTAGE_SCALE1);

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
  RCC_OscInitStruct.HSEState = RCC_HSE_ON;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
  RCC_OscInitStruct.PLL.PLLM = 4;
  RCC_OscInitStruct.PLL.PLLN = 168;
  RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV2;
  RCC_OscInitStruct.PLL.PLLQ = 4;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }

  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV4;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV2;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_5) != HAL_OK)
  {
    Error_Handler();
  }
}

/**
  * @brief ADC1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_ADC1_Init(void)
{

  /* USER CODE BEGIN ADC1_Init 0 */

  /* USER CODE END ADC1_Init 0 */

  ADC_ChannelConfTypeDef sConfig = {0};

  /* USER CODE BEGIN ADC1_Init 1 */

  /* USER CODE END ADC1_Init 1 */

  /** Configure the global features of the ADC (Clock, Resolution, Data Alignment and number of conversion)
  */
  hadc1.Instance = ADC1;
  hadc1.Init.ClockPrescaler = ADC_CLOCK_SYNC_PCLK_DIV4;
  hadc1.Init.Resolution = ADC_RESOLUTION_12B;
  hadc1.Init.ScanConvMode = ENABLE;
  hadc1.Init.ContinuousConvMode = ENABLE;
  hadc1.Init.DiscontinuousConvMode = DISABLE;
  hadc1.Init.ExternalTrigConvEdge = ADC_EXTERNALTRIGCONVEDGE_NONE;
  hadc1.Init.ExternalTrigConv = ADC_SOFTWARE_START;
  hadc1.Init.DataAlign = ADC_DATAALIGN_RIGHT;
  hadc1.Init.NbrOfConversion = 2;
  hadc1.Init.DMAContinuousRequests = ENABLE;
  hadc1.Init.EOCSelection = ADC_EOC_SINGLE_CONV;
  if (HAL_ADC_Init(&hadc1) != HAL_OK)
  {
    Error_Handler();
  }

  /** Configure for the selected ADC regular channel its corresponding rank in the sequencer and its sample time.
  */
  sConfig.Channel = ADC_CHANNEL_0;
  sConfig.Rank = 1;
  sConfig.SamplingTime = ADC_SAMPLETIME_144CYCLES;
  if (HAL_ADC_ConfigChannel(&hadc1, &sConfig) != HAL_OK)
  {
    Error_Handler();
  }

  /** Configure for the selected ADC regular channel its corresponding rank in the sequencer and its sample time.
  */
  sConfig.Channel = ADC_CHANNEL_1;
  sConfig.Rank = 2;
  if (HAL_ADC_ConfigChannel(&hadc1, &sConfig) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN ADC1_Init 2 */

  /* USER CODE END ADC1_Init 2 */

}

/**
  * @brief DAC Initialization Function
  * @param None
  * @retval None
  */
static void MX_DAC_Init(void)
{

  /* USER CODE BEGIN DAC_Init 0 */

  /* USER CODE END DAC_Init 0 */

  DAC_ChannelConfTypeDef sConfig = {0};

  /* USER CODE BEGIN DAC_Init 1 */

  /* USER CODE END DAC_Init 1 */

  /** DAC Initialization
  */
  hdac.Instance = DAC;
  if (HAL_DAC_Init(&hdac) != HAL_OK)
  {
    Error_Handler();
  }

  /** DAC channel OUT1 config
  */
  sConfig.DAC_Trigger = DAC_TRIGGER_NONE;
  sConfig.DAC_OutputBuffer = DAC_OUTPUTBUFFER_ENABLE;
  if (HAL_DAC_ConfigChannel(&hdac, &sConfig, DAC_CHANNEL_1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN DAC_Init 2 */

  /* USER CODE END DAC_Init 2 */

}

/**
  * @brief TIM1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_TIM1_Init(void)
{

  /* USER CODE BEGIN TIM1_Init 0 */

  /* USER CODE END TIM1_Init 0 */

  TIM_ClockConfigTypeDef sClockSourceConfig = {0};
  TIM_MasterConfigTypeDef sMasterConfig = {0};
  TIM_OC_InitTypeDef sConfigOC = {0};
  TIM_BreakDeadTimeConfigTypeDef sBreakDeadTimeConfig = {0};

  /* USER CODE BEGIN TIM1_Init 1 */

  /* USER CODE END TIM1_Init 1 */
  htim1.Instance = TIM1;
  htim1.Init.Prescaler = 0;
  htim1.Init.CounterMode = TIM_COUNTERMODE_UP;
  htim1.Init.Period = 65535;
  htim1.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
  htim1.Init.RepetitionCounter = 0;
  htim1.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_DISABLE;
  if (HAL_TIM_Base_Init(&htim1) != HAL_OK)
  {
    Error_Handler();
  }
  sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
  if (HAL_TIM_ConfigClockSource(&htim1, &sClockSourceConfig) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_Init(&htim1) != HAL_OK)
  {
    Error_Handler();
  }
  sMasterConfig.MasterOutputTrigger = TIM_TRGO_RESET;
  sMasterConfig.MasterSlaveMode = TIM_MASTERSLAVEMODE_DISABLE;
  if (HAL_TIMEx_MasterConfigSynchronization(&htim1, &sMasterConfig) != HAL_OK)
  {
    Error_Handler();
  }
  sConfigOC.OCMode = TIM_OCMODE_PWM1;
  sConfigOC.Pulse = 0;
  sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
  sConfigOC.OCNPolarity = TIM_OCNPOLARITY_HIGH;
  sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
  sConfigOC.OCIdleState = TIM_OCIDLESTATE_RESET;
  sConfigOC.OCNIdleState = TIM_OCNIDLESTATE_RESET;
  if (HAL_TIM_PWM_ConfigChannel(&htim1, &sConfigOC, TIM_CHANNEL_1) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_ConfigChannel(&htim1, &sConfigOC, TIM_CHANNEL_2) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_ConfigChannel(&htim1, &sConfigOC, TIM_CHANNEL_3) != HAL_OK)
  {
    Error_Handler();
  }
  sBreakDeadTimeConfig.OffStateRunMode = TIM_OSSR_DISABLE;
  sBreakDeadTimeConfig.OffStateIDLEMode = TIM_OSSI_DISABLE;
  sBreakDeadTimeConfig.LockLevel = TIM_LOCKLEVEL_OFF;
  sBreakDeadTimeConfig.DeadTime = 0;
  sBreakDeadTimeConfig.BreakState = TIM_BREAK_DISABLE;
  sBreakDeadTimeConfig.BreakPolarity = TIM_BREAKPOLARITY_HIGH;
  sBreakDeadTimeConfig.AutomaticOutput = TIM_AUTOMATICOUTPUT_DISABLE;
  if (HAL_TIMEx_ConfigBreakDeadTime(&htim1, &sBreakDeadTimeConfig) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN TIM1_Init 2 */

  /* USER CODE END TIM1_Init 2 */
  HAL_TIM_MspPostInit(&htim1);

}

/**
  * @brief TIM2 Initialization Function
  * @param None
  * @retval None
  */
static void MX_TIM2_Init(void)
{

  /* USER CODE BEGIN TIM2_Init 0 */

  /* USER CODE END TIM2_Init 0 */

  TIM_ClockConfigTypeDef sClockSourceConfig = {0};
  TIM_MasterConfigTypeDef sMasterConfig = {0};

  /* USER CODE BEGIN TIM2_Init 1 */

  /* USER CODE END TIM2_Init 1 */
  htim2.Instance = TIM2;
  htim2.Init.Prescaler = 0;
  htim2.Init.CounterMode = TIM_COUNTERMODE_UP;
  htim2.Init.Period = 839;
  htim2.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
  htim2.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_DISABLE;
  if (HAL_TIM_Base_Init(&htim2) != HAL_OK)
  {
    Error_Handler();
  }
  sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
  if (HAL_TIM_ConfigClockSource(&htim2, &sClockSourceConfig) != HAL_OK)
  {
    Error_Handler();
  }
  sMasterConfig.MasterOutputTrigger = TIM_TRGO_RESET;
  sMasterConfig.MasterSlaveMode = TIM_MASTERSLAVEMODE_DISABLE;
  if (HAL_TIMEx_MasterConfigSynchronization(&htim2, &sMasterConfig) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN TIM2_Init 2 */

  /* USER CODE END TIM2_Init 2 */

}

/**
  * @brief UART4 Initialization Function
  * @param None
  * @retval None
  */
static void MX_UART4_Init(void)
{

  /* USER CODE BEGIN UART4_Init 0 */

  /* USER CODE END UART4_Init 0 */

  /* USER CODE BEGIN UART4_Init 1 */

  /* USER CODE END UART4_Init 1 */
  huart4.Instance = UART4;
  huart4.Init.BaudRate = 115200;
  huart4.Init.WordLength = UART_WORDLENGTH_8B;
  huart4.Init.StopBits = UART_STOPBITS_1;
  huart4.Init.Parity = UART_PARITY_NONE;
  huart4.Init.Mode = UART_MODE_TX_RX;
  huart4.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  huart4.Init.OverSampling = UART_OVERSAMPLING_16;
  if (HAL_UART_Init(&huart4) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN UART4_Init 2 */

  /* USER CODE END UART4_Init 2 */

}

/**
  * Enable DMA controller clock
  */
static void MX_DMA_Init(void)
{

  /* DMA controller clock enable */
  __HAL_RCC_DMA2_CLK_ENABLE();

  /* DMA interrupt init */
  /* DMA2_Stream0_IRQn interrupt configuration */
  HAL_NVIC_SetPriority(DMA2_Stream0_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(DMA2_Stream0_IRQn);

}

/**
  * @brief GPIO Initialization Function
  * @param None
  * @retval None
  */
static void MX_GPIO_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStruct = {0};
/* USER CODE BEGIN MX_GPIO_Init_1 */
/* USER CODE END MX_GPIO_Init_1 */

  /* GPIO Ports Clock Enable */
  __HAL_RCC_GPIOH_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();
  __HAL_RCC_GPIOE_CLK_ENABLE();
  __HAL_RCC_GPIOD_CLK_ENABLE();
  __HAL_RCC_GPIOC_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOA, GPIO_PIN_7, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOB, GPIO_PIN_0|GPIO_PIN_1, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOD, GPIO_PIN_10|GPIO_PIN_12|GPIO_PIN_13|GPIO_PIN_14
                          |GPIO_PIN_15, GPIO_PIN_RESET);

  /*Configure GPIO pin : PA7 */
  GPIO_InitStruct.Pin = GPIO_PIN_7;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);

  /*Configure GPIO pins : PB0 PB1 */
  GPIO_InitStruct.Pin = GPIO_PIN_0|GPIO_PIN_1;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

  /*Configure GPIO pins : PD10 PD12 PD13 PD14
                           PD15 */
  GPIO_InitStruct.Pin = GPIO_PIN_10|GPIO_PIN_12|GPIO_PIN_13|GPIO_PIN_14
                          |GPIO_PIN_15;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOD, &GPIO_InitStruct);

/* USER CODE BEGIN MX_GPIO_Init_2 */
/* USER CODE END MX_GPIO_Init_2 */
}

/* USER CODE BEGIN 4 */
void SaveCalibrationData(const CalibrationData_t *data)
{
    HAL_FLASH_Unlock();

    // Sector Erase
    FLASH_EraseInitTypeDef EraseInitStruct;
    uint32_t SectorError;

    EraseInitStruct.TypeErase = FLASH_TYPEERASE_SECTORS;
    EraseInitStruct.VoltageRange = FLASH_VOLTAGE_RANGE_3;
    EraseInitStruct.Sector = FLASH_USER_SECTOR;
    EraseInitStruct.NbSectors = 1;

    if (HAL_FLASHEx_Erase(&EraseInitStruct, &SectorError) != HAL_OK)
    {
        HAL_FLASH_Lock();
        return;
    }

    const uint32_t *pData = (const uint32_t *)data;
    for (uint32_t i = 0; i < sizeof(CalibrationData_t)/4; i++)
    {
        HAL_FLASH_Program(FLASH_TYPEPROGRAM_WORD, FLASH_USER_START_ADDR + i*4, pData[i]);
    }

    HAL_FLASH_Lock();
}

void LoadCalibrationData(CalibrationData_t *data)
{
    const uint32_t *pData = (const uint32_t *)FLASH_USER_START_ADDR;

    for (uint32_t i = 0; i < sizeof(CalibrationData_t)/4; i++)
    {
        ((uint32_t *)data)[i] = pData[i];
    }

    // Eğer hiç yazılmamışsa default değer ata
    if (data->valid_flag != 0xA5A5A5A5)
    {
        data->adc_offset = 0.0f;
        data->gain_correction = 0.911f;
        //yeni parametre eklenirse
        //@add_param
        data->valid_flag = 0xA5A5A5A5;
    }
}

void HAL_ADC_ConvCpltCallback(ADC_HandleTypeDef* hadc)
{
    adc_raw = ADC_VAL[0];
    adc_ch1 = ADC_VAL[1];
}


void UART4_IRQHandler(void)
{//timeout eklenecek
	if (UART4->SR & USART_SR_RXNE)
		{
		ReceiveData = (uint8_t)UART4->DR;
    	switch(rx_index)
    	{
    	case 0 ://Prefix Control
    		if(ReceiveData == 0x48)
    		{
    			rx_buffer[0] = ReceiveData;
    			bcc_rx^=rx_buffer[0];
				rx_index=1;
    		}
    		break;
    	case 1://Receive Header
    			rx_buffer[1] = ReceiveData;
    			bcc_rx^=rx_buffer[1];
    			rx_index=2;
    		break;
    	case 2://Length
    		rx_buffer[2] = ReceiveData;
    		bcc_rx^=rx_buffer[2];
    		data_length = rx_buffer[2];
    		if(data_length == 0x00)
    		{
    			rx_index=4;//Go BCC
    		}
    		else
    		{
    			rx_index=3; //Go Data
    		}
    		break;
    	case 3://Data
    		rx_buffer[3 + data_index] = ReceiveData;
    		bcc_rx ^= rx_buffer[3 + data_index];
    		data_index++;
    		if(data_index >= data_length)
    		{
    			rx_index=4;//Data Finsh Go BCC
    		}
    		break;
    	case 4://BCC
    		if(bcc_rx == ReceiveData)
    		{
    			rx_index=5;//End Bit Control
    		}
    		else
    		{
    			Reset_Rx_Buffer();
    		}
    		break;
    	case 5://End Bit Control
    		if(ReceiveData == 0x0A)
    		{
    			procces_rx_message(rx_buffer);
    			Reset_Rx_Buffer();
    		}
    		else
    		{
    			Reset_Rx_Buffer();
    		}
    		break;
    	default:
    		Reset_Rx_Buffer();
    		break;
    	}
    	//HAL_UART_Receive_IT(huart, &ReceiveData, 1);
    }

}
// 0x48    0x01    0x08    D1-0   D1-1    D1-2    D1-3    D2-0    D2-1    D2-2    D2-3    BRC    0x0A //Gain ve Ofseti Güncelle
// 0x48    0x02    0x08    D1-0   D1-1    D1-2    D1-3    D2-0    D2-1    D2-2    D2-3    BCC    0x0A  //Write Flash MEM
void procces_rx_message(uint8_t *data)
{
	switch (data[1])
	{
		case 0x01://update Offset and Gain
		{
			if(data[2] != 0x08)
				return; // Beklenmeyen uzunluk
			uint32_t offset_raw =
		        (data[3]) |
		        (data[4]<<8) |
		        (data[5]<<16) |
		        (data[6]<<24);

			uint32_t gain_raw =
		        (data[7]) |
		        (data[8]<<8) |
		        (data[9]<<16) |
		        (data[10]<<24);
			memcpy(&new_offset, &offset_raw, 4);
			memcpy(&new_gain, &gain_raw, 4);
			calib_data.adc_offset = new_offset;
			calib_data.gain_correction = new_gain;
		}
			break;
		case 0x02://write Gain and Offset for Flash Memory
			if(data[2] != 0x08)
				return; // Beklenmeyen uzunluk
			uint32_t offset_raw =
		        (data[3]) |
		        (data[4]<<8) |
		        (data[5]<<16) |
		        (data[6]<<24);

			uint32_t gain_raw =
		        (data[7]) |
		        (data[8]<<8) |
		        (data[9]<<16) |
		        (data[10]<<24);
			memcpy(&new_offset, &offset_raw, 4);
			memcpy(&new_gain, &gain_raw, 4);
			SaveCalibrationData(&calib_data);
			break;
		case 0x03://Query
			if(data[2] != 0x02)
				return;
			send_voltage_packet(adc_raw,V_real,calib_data.adc_offset,calib_data.gain_correction);
			DAC_Val = (data[3] << 8) | data[4];
			break;
		default:
			break;
	}
}

void Reset_Rx_Buffer(void)
{
	bcc_rx=0;	rx_index=0;		data_length=0;		data_index=0;
	memset(rx_buffer, 0, sizeof(rx_buffer));
}

void send_voltage_packet(uint32_t raw_data,float real_data, float offset, float gain)
{
	uint8_t SendVal [30];
	uint8_t bcc=0;
	bcc=0;
	SendVal[0]=0x48;//prefix
	SendVal[1]=0x01;//header
	SendVal[2]=0x14;//MessageLenght
	bcc^=(SendVal[0]);
	bcc^=(SendVal[1]);
	bcc^=(SendVal[2]);

	SendVal[3]=(uint8_t)(adc_raw & 0xFF); 		bcc^=(SendVal[3]);
	SendVal[4]=(uint8_t)(adc_raw>>8 & 0xFF);	bcc^=(SendVal[4]);
	SendVal[5]=(uint8_t)(adc_raw>>16 & 0xFF);	bcc^=(SendVal[5]);
	SendVal[6]=(uint8_t)(adc_raw>>24 & 0xFF);	bcc^=(SendVal[6]);

	SendVal[7]=(uint8_t)(adc_ch1 & 0xFF); 		bcc^=(SendVal[7]);
	SendVal[8]=(uint8_t)(adc_ch1>>8 & 0xFF);	bcc^=(SendVal[8]);
	SendVal[9]=(uint8_t)(adc_ch1>>16 & 0xFF);	bcc^=(SendVal[9]);
	SendVal[10]=(uint8_t)(adc_ch1>>24 & 0xFF);	bcc^=(SendVal[10]);

	uint8_t *float_byte = (uint8_t*)&V_real;
	SendVal[11] = float_byte[0];				bcc^=(SendVal[11]);
	SendVal[12] = float_byte[1];				bcc^=(SendVal[12]);
	SendVal[13] = float_byte[2];				bcc^=(SendVal[13]);
	SendVal[14] = float_byte[3];				bcc^=(SendVal[14]);

	uint8_t *float_byte1 = (uint8_t*)&offset;
	SendVal[15] = float_byte1[0];				bcc^=(SendVal[15]);
	SendVal[16] = float_byte1[1];				bcc^=(SendVal[16]);
	SendVal[17] = float_byte1[2];				bcc^=(SendVal[17]);
	SendVal[18] = float_byte1[3];				bcc^=(SendVal[18]);

	uint8_t *float_byte2 = (uint8_t*)&gain;
	SendVal[19] = float_byte2[0];				bcc^=(SendVal[19]);
	SendVal[20] = float_byte2[1];				bcc^=(SendVal[20]);
	SendVal[21] = float_byte2[2];				bcc^=(SendVal[21]);
	SendVal[22] = float_byte2[3];				bcc^=(SendVal[22]);

	SendVal[23] = bcc;
	SendVal[24] = 0x0A;
	HAL_UART_Transmit(&huart4, SendVal, 25, 10);
}

void calculate_adc_val(void)
{
	  if(adc_raw > calib_data.adc_offset)
	  	    adc_raw -= calib_data.adc_offset;
	   else
	  	    adc_raw = 0;

	  VoltageVAl+=adc_raw;
	  VoltageVAl-=Voltage_Filt;
	  Voltage_Filt=VoltageVAl>>8;

	  V_meas = (3.3f * Voltage_Filt) / 4095.0f;
	  V_real = V_meas * 11.0f * calib_data.gain_correction;
}
/* USER CODE END 4 */

/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

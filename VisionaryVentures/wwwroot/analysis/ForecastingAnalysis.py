import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import sys
from statsmodels.tsa.arima.model import ARIMA

# Adjust the command-line arguments as necessary
file_path = sys.argv[1]
time_column = sys.argv[2] if len(sys.argv) > 2 and sys.argv[2] != 'None' else None
forecast_column = sys.argv[3] if len(sys.argv) > 3 else None
independent_vars = sys.argv[4].split(',') if len(sys.argv) > 4 else []

def load_data(file_path):
    """Load the dataset into a pandas DataFrame."""
    return pd.read_excel(file_path)

def perform_arima_forecasting(df, forecast_column):
    """Perform ARIMA forecasting on time series data."""
    model = ARIMA(df[forecast_column], order=(1, 1, 1))
    model_fit = model.fit()
    forecast = model_fit.forecast(steps=12)
    return forecast

def custom_forecast_based_on_averages(df, independent_vars):
    """Simple custom forecasting based on the average of independent variables."""
    if not independent_vars:
        raise ValueError("Independent variables not specified for custom forecasting.")
    
    # Calculate the average of independent variables
    df['Average'] = df[independent_vars].mean(axis=1)
    forecast_value = df['Average'].iloc[-1]  # Placeholder for a forecasting logic
    return np.full((12,), forecast_value)  # Simulating 12 steps forecast with the same value

def main():
    df = load_data(file_path)

    plt.figure(figsize=(10, 6))

    if time_column and time_column in df.columns:
        df.set_index(time_column, inplace=True)
        df.index = pd.to_datetime(df.index)
        forecast = perform_arima_forecasting(df, forecast_column)
        plt.plot(df.index, df[forecast_column], label='Actual Data', color='blue')
        plt.plot(forecast.index, forecast, label='Forecast', color='red')
    else:
        forecast = custom_forecast_based_on_averages(df, independent_vars)
        steps = np.arange(len(forecast))
        plt.plot(steps, forecast, label='Forecast', color='red')
        plt.xticks(steps, labels=[f"Step {i+1}" for i in steps])  # Label steps as Step 1, Step 2, etc.

    plt.legend()
    plt.savefig('wwwroot/charts/forecast_chart.png')

if __name__ == "__main__":
    main()

import pandas as pd
import statsmodels.api as sm
import matplotlib.pyplot as plt
import sys

file_path = sys.argv[1]
dependent_var = sys.argv[2]
independent_vars = sys.argv[3].split(',')

def load_data(file_path):
    """Load Excel data into a pandas DataFrame."""
    return pd.read_excel(file_path)

def select_data(df, dependent_var, independent_vars, filters={}):
    """Select and filter data based on user input."""
    # Apply filters if any
    for column, value in filters.items():
        df = df[df[column] == value]
    X = df[independent_vars]
    y = df[dependent_var]
    return X, y

def perform_regression(X, y, add_intercept=True):
    """Perform regression analysis."""
    if add_intercept:
        X = sm.add_constant(X)
    model = sm.OLS(y, X).fit()
    return model

def generate_report_and_chart(model, X, y, output_path):
    print(model.summary())
    with open(output_path, 'w') as f:
        # Write the model summary to the file
        f.write(str(model.summary()))
        # You can also add more information to the file as needed

    # Assuming the first independent var for the x-axis for simplicity
    fig, ax = plt.subplots()
    ax.scatter(X.iloc[:, 0], y)
    ax.plot(X.iloc[:, 0], model.predict(sm.add_constant(X)), color='red')
    
    # Save the plot. Ensure the directory exists or this will fail.
    plt.savefig('wwwroot/charts/plot.png')

def main():
    output_path = 'wwwroot/analysis_output/AnalysisOutput.txt'

    # Load data
    df = load_data(file_path)

    # Select data
    X, y = select_data(df, dependent_var, independent_vars)

    # Perform regression
    model = perform_regression(X, y)

    # Generate report and chart
    generate_report_and_chart(model, X, y, output_path)

if __name__ == "__main__":
    main()

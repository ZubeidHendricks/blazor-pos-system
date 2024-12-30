import os
from handlers.task_analyzer import analyze_issue
from handlers.code_generator import generate_code

def main():
    # Run task analysis
    analyze_issue()
    
    # Generate code based on analysis
    with open('analysis/latest.txt', 'r') as f:
        analysis = eval(f.read())
    generate_code(analysis)

if __name__ == '__main__':
    main()
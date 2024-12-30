import os
import groq
from github import Github
from jinja2 import Template

def generate_code(analysis):
    groq_client = groq.Groq(api_key=os.getenv('GROQ_API_KEY'))
    
    # Generate implementation based on analysis
    implementation = generate_implementation(groq_client, analysis)
    
    # Generate tests
    tests = generate_tests(groq_client, analysis)
    
    # Save generated code
    save_code(implementation, tests, analysis)

def generate_implementation(client, analysis):
    prompt = f"""
    Generate C# implementation code for this task:

    Analysis: {analysis['requirements']}
    Type: {analysis['type']}

    Include:
    1. Complete class implementation
    2. All required methods
    3. Error handling
    4. Comments and documentation
    """
    
    response = client.chat.completions.create(
        messages=[{
            "role": "system",
            "content": "You are an expert C# developer specializing in Blazor applications."
        }, {
            "role": "user",
            "content": prompt
        }],
        model="mixtral-8x7b-32768",
        temperature=0.2,
        max_tokens=2000
    )
    
    return response.choices[0].message.content

def generate_tests(client, analysis):
    prompt = f"""
    Generate unit and integration tests for this C# implementation:

    Analysis: {analysis['requirements']}
    Type: {analysis['type']}

    Include:
    1. Unit tests for all methods
    2. Integration tests
    3. Mock setups
    4. Test data preparation
    """
    
    response = client.chat.completions.create(
        messages=[{
            "role": "system",
            "content": "You are an expert in C# testing using xUnit."
        }, {
            "role": "user",
            "content": prompt
        }],
        model="mixtral-8x7b-32768",
        temperature=0.2,
        max_tokens=2000
    )
    
    return response.choices[0].message.content

def save_code(implementation, tests, analysis):
    task_name = analysis['title'].lower().replace(' ', '_')
    
    # Save implementation
    os.makedirs('src/BlazorPOS.Core/Features', exist_ok=True)
    with open(f'src/BlazorPOS.Core/Features/{task_name}.cs', 'w') as f:
        f.write(implementation)
    
    # Save tests
    os.makedirs('tests/BlazorPOS.Tests', exist_ok=True)
    with open(f'tests/BlazorPOS.Tests/{task_name}_tests.cs', 'w') as f:
        f.write(tests)

if __name__ == '__main__':
    with open('analysis/latest.txt', 'r') as f:
        analysis = eval(f.read())
    generate_code(analysis)
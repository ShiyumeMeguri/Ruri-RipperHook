
This project modifies the AR loading mechanism using IlHook to achieve decryption. To minimize unnecessary repetitive work, the module is designed with fine granularity. Each decryption process should be as generic as possible and allow flexible insertion of decryption code.  

However, since **ds5678** enjoys refactoring the code too much, I no longer wish to maintain this project.  

## TODO  
- Automatically read the official `typetree.json` and the game's custom engine `typetree.json`, compare the differences, and generate a difference tree to automate `ClassHook`.  

## Special Thanks to:  
- **ds5678**: Original author.  
- **Razmoth**: For anything.  

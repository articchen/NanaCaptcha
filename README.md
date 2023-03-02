# NanaCaptcha
C# RESTFul API implements Captcha function

Very simple approach.

Send the picture and the encrypted Captcha number to the front end such as Angular. Then, when confirming the Captcha, the front end sends the encrypted number together with the number entered by the user to the API for Check.

The encryption algorithm uses the simplest XOR, and you can change it to an encryption algorithm such as DES or RSA by yourself.

Currently, the drawing method only supports IIS Server, and it will be changed to a cross-platform drawing method in the future.

2023.03.02
Arthur

[Angular example](https://angular-kx77c9.stackblitz.io)

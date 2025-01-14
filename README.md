## Goal
In this repository I'm going to explore ocerisation technologies via python, java and C#.

# OCR
OCR (Optical Character Recognition) is a technology that enables the automatic recognition and conversion of text within images, scanned documents, or handwritten notes into machine-readable text. It works by analyzing the visual patterns in an image to identify characters, words, and sometimes entire sentences, using methods like pattern matching, feature extraction, and machine learning. Modern OCR systems often leverage advanced deep learning techniques to handle a wide range of text styles, fonts, and languages, even in challenging conditions like distorted or low-quality images. OCR is widely used in applications such as digitizing printed materials, extracting data from forms, enabling searchable document archives, and powering assistive technologies for visually impaired users.

# Python
EasyOCR is a Python library that provides an easy-to-use interface for Optical Character Recognition (OCR). It supports over 80 languages and is particularly effective for recognizing text in images, including handwritten text and complex fonts. EasyOCR leverages deep learning models for high accuracy and can be quickly integrated into Python projects for text extraction from images or PDFs.

# CSharp
IronOCR is a commercial OCR library for .NET, offering high accuracy and performance in text recognition from images and PDFs. It supports various languages and file formats, including PDFs, and is known for handling noisy or complex images better than many other OCR tools.\n
Tesseract, on the other hand, is an open-source OCR engine widely used in many languages, including C#. It supports over 100 languages and is highly customizable, though it may require more setup and image preprocessing for optimal results, especially in low-quality images.

# Java
Tess4J is a Java wrapper for the open-source OCR engine Tesseract, enabling developers to easily integrate text recognition capabilities into Java applications. It supports over 100 languages and can extract text from images in various formats such as PNG, JPG, and TIFF. Tess4J provides a simple API for performing OCR tasks, making it a popular choice for text extraction from scanned documents and images.

# EasyOCR Python
EasyOCR is built around three main components: feature extraction, sequence labeling, and decoding.

Feature Extraction:
    Deep learning models like ResNet and VGG are used to extract meaningful features from input images.
    ResNet: Uses skip connections to address vanishing gradient issues, allowing deeper networks and easier learning of residual functions.
    VGG: Employs a simple and uniform architecture with small (3x3) convolution filters, offering clarity and ease of modification.

Sequence Labeling:
    This stage uses Long Short-Term Memory (LSTM) networks to model the sequential context of the extracted features. LSTMs handle long sequences efficiently, overcoming the limitations of traditional RNNs through their unique architecture with memory cells and gates.

Decoding:
    The final stage employs Connectionist Temporal Classification (CTC) to convert labeled sequences into recognized text. CTC is designed for sequence tasks with uncertain alignments, making it well-suited for OCR by summing over all possible alignments of the input to target sequences.

The training pipeline of EasyOCR is based on the deep-text-recognition-benchmark framework, enabling robust text recognition across diverse datasets.

The libraries for coding are : torch, torchvision, torchaudio, easyocr

# LSTM
An LSTM (Long Short-Term Memory) is a type of recurrent neural network (RNN) designed to process and learn from sequential data by addressing the limitations of traditional RNNs, such as the vanishing gradient problem. It achieves this through a unique architecture that includes a memory cell and three gates: the input gate, which determines what new information to store; the forget gate, which decides what information to discard; and the output gate, which controls what information to use as output. This structure enables LSTMs to capture long-term dependencies and contextual relationships in sequences, making them ideal for tasks like speech recognition, machine translation, optical character recognition (OCR), and time-series forecasting.

# Reference 
- Python : EasyOCR : https://www.jaided.ai/easyocr/documentation/
- Python article : https://medium.com/@adityamahajan.work/easyocr-a-comprehensive-guide-5ff1cb850168
- CSharp : IronOCR : https://ironsoftware.com/csharp/ocr/docs/
- CSharp : Tesseract : https://ironsoftware.com/fr/csharp/ocr/tutorials/c-sharp-tesseract-ocr/
- Java : Tess4j : https://javadoc.io/doc/net.sourceforge.tess4j/tess4j/latest/index.html

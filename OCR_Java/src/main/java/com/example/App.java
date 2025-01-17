package com.example;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.stream.Collectors;

import org.apache.commons.io.IOUtils;
import org.tartarus.snowball.ext.PorterStemmer;

import net.sourceforge.tess4j.ITesseract;
import net.sourceforge.tess4j.Tesseract;

public class App {

    /**
     * Computes the TF-IDF score between two texts.
     *
     * @param pathText1   The path to the first text file (reference).
     * @param pathText2   The path to the second text file (translated).
     * @return            The TF-IDF score rounded to 2 decimal places.
     */
    public static double computeTFIDF(String pathText1, String pathText2) {
        try {
            // Load and preprocess texts
            String text1 = loadAndPreprocessText(pathText1);
            String text2 = loadAndPreprocessText(pathText2);

            // Tokenize the texts into words
            List<String> words1 = tokenize(text1);
            List<String> words2 = tokenize(text2);

            // Create a combined vocabulary
            Set<String> vocabulary = new HashSet<>();
            vocabulary.addAll(words1);
            vocabulary.addAll(words2);

            // Compute term frequencies for both texts
            Map<String, Integer> termFrequency1 = computeTermFrequency(words1);
            Map<String, Integer> termFrequency2 = computeTermFrequency(words2);

            // Compute IDF scores for the vocabulary
            Map<String, Double> inverseDocumentFrequency = computeIDF(Arrays.asList(words1, words2), vocabulary);

            // Calculate TF-IDF score
            double tfidfScore = 0.0;
            for (String term : vocabulary) {
                double tf1 = termFrequency1.getOrDefault(term, 0);
                double tf2 = termFrequency2.getOrDefault(term, 0);
                tfidfScore += (tf1 * inverseDocumentFrequency.get(term)) + (tf2 * inverseDocumentFrequency.get(term));
            }

            // Round the score to 2 decimal places
            return Math.round(tfidfScore * 100.0) / 100.0;

        } catch (IOException e) {
            System.err.println("Error processing texts: " + e.getMessage());
            e.printStackTrace();
            return 0.0;
        }
    }

    /**
     * Loads and preprocesses a text from a given file path.
     */
    private static String loadAndPreprocessText(String path) throws IOException {
        File file = new File(path);
        String content = IOUtils.toString(new FileInputStream(file), StandardCharsets.UTF_8);

        // Basic text preprocessing (removal of non-alphanumeric characters and lowercasing)
        content = content.replaceAll("[^a-zA-Z\\s]", "").toLowerCase();
        return content;
    }

    /**
     * Tokenizes a text into words.
     */
    private static List<String> tokenize(String text) {
        String[] words = text.split("\\s+");
        return Arrays.stream(words)
                .filter(word -> !word.isEmpty())
                .map(App::stem)
                .collect(Collectors.toList());
    }

    /**
     * Computes the term frequency (TF) for a list of words.
     */
    private static Map<String, Integer> computeTermFrequency(List<String> words) {
        Map<String, Integer> termFrequency = new HashMap<>();
        for (String word : words) {
            termFrequency.put(word, termFrequency.getOrDefault(word, 0) + 1);
        }
        return termFrequency;
    }

    /**
     * Computes the inverse document frequency (IDF) for a vocabulary.
     */
    private static Map<String, Double> computeIDF(List<List<String>> documents, Set<String> vocabulary) {
        Map<String, Integer> documentFrequency = new HashMap<>();
        for (String term : vocabulary) {
            int count = 0;
            for (List<String> document : documents) {
                if (document.contains(term)) {
                    count++;
                }
            }
            documentFrequency.put(term, count);
        }

        int totalDocuments = documents.size();
        Map<String, Double> inverseDocumentFrequency = new HashMap<>();
        for (Map.Entry<String, Integer> entry : documentFrequency.entrySet()) {
            double idf = Math.log((double) totalDocuments / entry.getValue());
            inverseDocumentFrequency.put(entry.getKey(), idf);
        }

        return inverseDocumentFrequency;
    }

    /**
     * Stems a word using the Porter Stemmer.
     */
    private static String stem(String word) {
        PorterStemmer stemmer = new PorterStemmer();
        stemmer.setCurrent(word);
        stemmer.stem();
        return stemmer.getCurrent();
    }

    /**
     * Performs OCR on an image and saves the extracted text to a result file.
     *
     * @param pathImage The path to the image file containing the text to be extracted.
     * @param pathResult The path to the file where the extracted text will be saved.
     *
     * This function loads the Tesseract data files, performs OCR on the image, and writes
     * the extracted text to the specified result file. If an error occurs during OCR or file
     * saving, it will be printed to the console.
     */
    public static void extractTextAndSave(String pathImage, String pathResult) {
        File tessDataFolder = new File("C:\\Program Files\\Tesseract-OCR\\tessdata");

        ITesseract instance = new Tesseract();
        instance.setLanguage("eng+fra");

        instance.setDatapath(tessDataFolder.getAbsolutePath());

        try {
            File imageFile = new File(pathImage);
            String result = instance.doOCR(imageFile);

            File resultFile = new File(pathResult);
            if (!resultFile.exists()) {
                resultFile.getParentFile().mkdirs();
                resultFile.createNewFile();
            }

            try (FileWriter writer = new FileWriter(resultFile)) {
                writer.write(result);
            }
            System.out.println("The extracted text has been saved to " + pathResult);

        } catch (Exception e) {
            System.err.println("Error during OCR or file saving: " + e.getMessage());
            e.printStackTrace();
        }
    }

    public static void main(String[] args) {
        String baseDir = "E:\\Github\\OCR_Python_CSharp_Java\\OCR_Java";
        File resultDir = new File(baseDir, "result_ocr_java");

        File file = new File("OCR_Items\\test_image.jpeg");
        String memePath = file.getAbsolutePath();

        File file2 = new File("OCR_Items\\test_image.txt");
        String memeTraductionPath = file2.getAbsolutePath();

        File file3 = new File(resultDir, "result_test_image.txt");
        String resultMemePath = file3.getAbsolutePath();

        File file4 = new File("OCR_Items\\test_OCR_1.jpg");
        String ocr1Path = file4.getAbsolutePath();

        File file5 = new File("OCR_Items\\test_OCR_trad_1.txt");
        String ocr1TraductionPath = file5.getAbsolutePath();

        File file6 = new File(resultDir, "result_test_ocr_1.txt");
        String resultOcr1Path = file6.getAbsolutePath();

        File file7 = new File("OCR_Items\\test_OCR_2.jpg");
        String ocr2Path = file7.getAbsolutePath();

        File file8 = new File("OCR_Items\\test_OCR_trad_2.txt");
        String ocr2TraductionPath = file8.getAbsolutePath();

        File file9 = new File(resultDir, "result_test_ocr_2.txt");
        String resultOcr2Path = file9.getAbsolutePath();

        extractTextAndSave(memePath, resultMemePath);
        extractTextAndSave(ocr1Path, resultOcr1Path);
        extractTextAndSave(ocr2Path, resultOcr2Path);

        System.out.println(computeTFIDF(ocr1TraductionPath,resultOcr1Path));
        System.out.println(computeTFIDF(ocr2TraductionPath,resultOcr2Path));
        System.out.println(computeTFIDF(memeTraductionPath,resultMemePath));
    }
}

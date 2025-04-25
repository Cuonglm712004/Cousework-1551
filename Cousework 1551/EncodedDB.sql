
CREATE TABLE EncodedStrings (
    id INT AUTO_INCREMENT PRIMARY KEY,                -- Unique ID for each record
    original_string VARCHAR(255) NOT NULL,            -- The original input string
    encoded_caesar VARCHAR(255),                      -- Result of Caesar Processor encoding
    encoded_reverse VARCHAR(255),                     -- Result of Reverse Processor encoding
    encoded_atbash VARCHAR(255),                      -- Result of Atbash Processor encoding
    encoded_mirror VARCHAR(255),                      -- Result of Mirror Processor
    encoded_vowel_replacer VARCHAR(255),              -- Result of Vowel Replacer Processor
    technique VARCHAR(255),                           -- Which encoding technique was applied
    encoded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP    -- Timestamp of when encoding happened
);


SHOW DATABASES;


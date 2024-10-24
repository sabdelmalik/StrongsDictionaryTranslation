﻿using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Postgresql2MySql
{
    internal class TableDefs
    {
        public static string[] Defs =
        {
            "CREATE TABLE IF NOT EXISTS bible_text" +
            "(" +
                "id INT NOT NULL AUTO_INCREMENT," +
                "language_id integer NOT NULL," +
                "book_id integer NOT NULL," +
                "chapter_num integer NOT NULL," +
                "verse_num integer NOT NULL," +
                "verse_text text," +
                "CONSTRAINT bible_text_pkey PRIMARY KEY (id)," +
                "CONSTRAINT bible_text_book_id_chapter_num_verse_num_key UNIQUE (book_id, chapter_num, verse_num)" +
            ");",

        "CREATE TABLE IF NOT EXISTS strongs_numbers" +
        "(" +
            "id INT NOT NULL AUTO_INCREMENT," +
            "strongs_number character(5) NOT NULL," +
            "d_strong character(1) BINARY," +
        "original_word character varying(128) NOT NULL," +
        "english_translation character varying(128) NOT NULL," +
        "long_text text," +
        "short_text text," +
        "step_united_reason character varying(32)," +
        "step_type character varying(32)," +
        "transliteration character varying(128)," +
        "pronunciation character varying(128)," +
        "CONSTRAINT StrongsNumbers_pkey PRIMARY KEY(id)," +
        "CONSTRAINT strong_numbers_unique UNIQUE(strongs_number, d_strong)" +
    ");",

    "CREATE TABLE IF NOT EXISTS bible_words_references " +
    "(" +
        "id INT NOT NULL AUTO_INCREMENT," +
        "language_id integer NOT NULL," +
        "strongs_number character(5) NOT NULL," +
        "d_strong character(1) BINARY," +
        "word character varying(64) BINARY NOT NULL," +
        "reference text," +
        "CONSTRAINT bible_words_references_pkey PRIMARY KEY(id)," +
        "CONSTRAINT bible_words_references_augmented_strong UNIQUE(language_id, strongs_number, d_strong, word)" +
    ");",

    "CREATE TABLE IF NOT EXISTS book " +
        "(" +
        "id integer NOT NULL," +
        "usfm_name character varying(64) NOT NULL," +
        "osis_name character varying(64)," +
        "full_name character varying(128)," +
        "CONSTRAINT book_pkey PRIMARY KEY(id)" +
    ");",

    "CREATE TABLE IF NOT EXISTS dictionary_translation " +
    "(" +
        "id INT NOT NULL AUTO_INCREMENT," +
        "language_id integer NOT NULL," +
        "strongs_number character(5) NOT NULL," +
        "d_strong character(1) BINARY," +
        "transliteration character varying(128)," +
        "translated_word character varying(128)," +
        "translated_long_text text," +
        "translated_short_text text," +
        "reviewed boolean DEFAULT FALSE," +
        "reviewer_initials character varying(8)," +
        "approved boolean DEFAULT FALSE," +
        "approver_initials character varying(8)," +
        "PRIMARY KEY(id)" +
    ");",

    "CREATE TABLE IF NOT EXISTS language " +
    "(" +
        "id integer NOT NULL," +
        "name character varying(128)," +
        "iso_639_1 character(2) NOT NULL," +
        "iso_639_2 character(3) NOT NULL," +
        "PRIMARY KEY(id)" +
    ");",

    "CREATE TABLE IF NOT EXISTS strongs_references " +
        "("+
        "id INT NOT NULL AUTO_INCREMENT," +
        "strongs_number character(5) NOT NULL," +
        "d_strong character(1) BINARY," +
        "reference text NOT NULL," +
        "CONSTRAINT Strongs_references_pkey PRIMARY KEY(id)," +
        "CONSTRAINT strongs_references_augmented_strong UNIQUE(strongs_number, d_strong)" +
    ");",

    "CREATE TABLE IF NOT EXISTS updated_translation " +
    "(" +
        "id INT NOT NULL AUTO_INCREMENT," +
        "language_id integer NOT NULL," +
        "strongs_number character(5) NOT NULL," +
        "d_strong character(1) BINARY," +
        "transliteration character varying(128)," +
        "translated_word character varying(128)," +
        "translated_text text," +
        "reviewer_initials character varying(32)," +
        "approved boolean DEFAULT FALSE," +
        "approver_initials character varying(32)," +
        "PRIMARY KEY(id)" +
    ");",

    "ALTER TABLE bible_text " +
        "ADD FOREIGN KEY(language_id) " +
        "REFERENCES language(id) MATCH SIMPLE " +
        "ON UPDATE NO ACTION " +
        "ON DELETE NO ACTION;",

        "ALTER TABLE bible_text " +
            "ADD FOREIGN KEY(book_id) " +
            "REFERENCES book(id) MATCH SIMPLE " +
            "ON UPDATE NO ACTION " +
            "ON DELETE NO ACTION;",

        "ALTER TABLE dictionary_translation " +
            "ADD FOREIGN KEY(language_id) " +
            "REFERENCES language(id) MATCH SIMPLE " +
            "ON UPDATE NO ACTION " +
            "ON DELETE NO ACTION;",


        "ALTER TABLE dictionary_translation " +
            "ADD FOREIGN KEY(strongs_number, d_strong) " +
            "REFERENCES strongs_numbers(strongs_number, d_strong) MATCH SIMPLE " +
            "ON UPDATE NO ACTION " +
            "ON DELETE NO ACTION;"
    };

    }
}

DROP TABLE IF EXISTS public."updated_translation";
CREATE TABLE IF NOT EXISTS public.updated_translation
(
    id bigserial NOT NULL,
    language_id integer NOT NULL,
    strongs_number character(5) NOT NULL,
    d_strong character(1),
    transliteration character varying(128),
    translated_word character varying(128),
    translated_text text,
    reviewer_initials character varying(32),
    approved boolean DEFAULT FALSE,
    approver_initials character varying(32),
    PRIMARY KEY (id)
);


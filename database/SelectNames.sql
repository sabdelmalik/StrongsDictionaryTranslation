select 
strongs.strongs_number,
strongs.original_word, 
strongs.english_translation,
strongs.transliteration,
google_words.translated_word,
svd_words.word

from public."strongs_numbers" strongs INNER JOIN public."bible_words_references" svd_words
    ON strongs.strongs_number = svd_words.strongs_number
	INNER JOIN public."dictionary_translation" google_words
	ON strongs.strongs_number = google_words.strongs_number
where strongs.step_type IN (SELECT DISTINCT  step_type from public."strongs_numbers" WHERE step_united_reason='named');
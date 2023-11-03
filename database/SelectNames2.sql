select 
strongs.strongs_number AS Strong,
strongs.d_strong As dStrong,
strongs.original_word AS GrkHeb, 
strongs.english_translation AS English,
google_words.translated_word AS Google,
svd_words.word As SVD,
google_words.translated_long_text As Text

from public."strongs_numbers" strongs INNER JOIN public."bible_words_references" svd_words
    ON strongs.strongs_number = svd_words.strongs_number
	INNER JOIN public."dictionary_translation" google_words
	ON strongs.strongs_number = google_words.strongs_number
where strongs.step_type IN (SELECT DISTINCT  step_type from public."strongs_numbers" WHERE step_united_reason='named');
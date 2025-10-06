import { Clock, BookOpen, TrendingUp } from 'lucide-react';
import { Course } from '../lib/supabase';

type CourseCardProps = {
  course: Course & { instructor_name?: string; lesson_count?: number };
  onEnroll?: (courseId: string) => void;
  onViewDetails?: (courseId: string) => void;
  enrolled?: boolean;
  progress?: number;
};

export const CourseCard = ({ course, onEnroll, onViewDetails, enrolled, progress }: CourseCardProps) => {
  const levelColors = {
    beginner: 'bg-green-100 text-green-700',
    intermediate: 'bg-yellow-100 text-yellow-700',
    advanced: 'bg-red-100 text-red-700',
  };

  const levelLabels = {
    beginner: 'Iniciante',
    intermediate: 'Intermediário',
    advanced: 'Avançado',
  };

  return (
    <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition overflow-hidden group">
      <div className="relative h-48 bg-gradient-to-br from-blue-500 to-blue-600 overflow-hidden">
        {course.thumbnail_url ? (
          <img
            src={course.thumbnail_url}
            alt={course.title}
            className="w-full h-full object-cover group-hover:scale-105 transition duration-300"
          />
        ) : (
          <div className="w-full h-full flex items-center justify-center">
            <BookOpen className="w-16 h-16 text-white opacity-50" />
          </div>
        )}
        <div className="absolute top-4 right-4">
          <span className={`px-3 py-1 rounded-full text-xs font-semibold ${levelColors[course.level]}`}>
            {levelLabels[course.level]}
          </span>
        </div>
      </div>

      <div className="p-6">
        <div className="flex items-center space-x-2 mb-3">
          <span className="px-3 py-1 bg-slate-100 text-slate-700 rounded-full text-xs font-medium">
            {course.category}
          </span>
          {course.lesson_count && (
            <span className="flex items-center text-gray-500 text-xs">
              <BookOpen className="w-3 h-3 mr-1" />
              {course.lesson_count} aulas
            </span>
          )}
        </div>

        <h3 className="text-xl font-bold text-gray-900 mb-2 line-clamp-2">
          {course.title}
        </h3>

        <p className="text-gray-600 text-sm mb-4 line-clamp-3">
          {course.description}
        </p>

        {enrolled && progress !== undefined && (
          <div className="mb-4">
            <div className="flex items-center justify-between mb-2">
              <span className="text-sm font-medium text-gray-700 flex items-center">
                <TrendingUp className="w-4 h-4 mr-1" />
                Progresso
              </span>
              <span className="text-sm font-bold text-blue-600">{progress}%</span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-2">
              <div
                className="bg-blue-600 h-2 rounded-full transition-all duration-300"
                style={{ width: `${progress}%` }}
              />
            </div>
          </div>
        )}

        <div className="flex items-center justify-between">
          {course.instructor_name && (
            <div className="flex items-center space-x-2">
              <div className="w-8 h-8 bg-gradient-to-br from-slate-200 to-slate-300 rounded-full flex items-center justify-center">
                <span className="text-xs font-bold text-slate-600">
                  {course.instructor_name.charAt(0).toUpperCase()}
                </span>
              </div>
              <span className="text-sm text-gray-600">{course.instructor_name}</span>
            </div>
          )}

          {onViewDetails && (
            <button
              onClick={() => onViewDetails(course.id)}
              className="px-4 py-2 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 transition"
            >
              {enrolled ? 'Continuar' : 'Ver detalhes'}
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

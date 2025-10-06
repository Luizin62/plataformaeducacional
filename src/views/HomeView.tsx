import { BookOpen, Users, Award, TrendingUp } from 'lucide-react';

type HomeViewProps = {
  onNavigateToCourses: () => void;
};

export const HomeView = ({ onNavigateToCourses }: HomeViewProps) => {
  const stats = [
    { icon: BookOpen, label: 'Cursos Disponíveis', value: '50+', color: 'bg-blue-500' },
    { icon: Users, label: 'Estudantes Ativos', value: '10k+', color: 'bg-green-500' },
    { icon: Award, label: 'Certificados Emitidos', value: '5k+', color: 'bg-yellow-500' },
    { icon: TrendingUp, label: 'Taxa de Conclusão', value: '85%', color: 'bg-red-500' },
  ];

  const features = [
    {
      title: 'Aprenda no seu ritmo',
      description: 'Acesse o conteúdo quando quiser, de onde estiver. Aprenda no seu próprio tempo.',
      icon: Clock,
    },
    {
      title: 'Instrutores experientes',
      description: 'Aprenda com profissionais que são referência em suas áreas de atuação.',
      icon: Users,
    },
    {
      title: 'Certificados reconhecidos',
      description: 'Receba certificados ao concluir os cursos e destaque-se no mercado.',
      icon: Award,
    },
  ];

  return (
    <div className="space-y-12">
      <section className="text-center py-12">
        <h1 className="text-5xl font-bold text-gray-900 mb-6">
          Transforme sua carreira com
          <span className="text-blue-600"> educação de qualidade</span>
        </h1>
        <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
          Acesse cursos online de alta qualidade e desenvolva as habilidades necessárias para alcançar seus objetivos profissionais.
        </p>
        <button
          onClick={onNavigateToCourses}
          className="px-8 py-4 bg-blue-600 text-white text-lg font-semibold rounded-xl hover:bg-blue-700 transition shadow-lg hover:shadow-xl"
        >
          Explorar Cursos
        </button>
      </section>

      <section className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat, index) => (
          <div
            key={index}
            className="bg-white rounded-xl shadow-sm p-6 hover:shadow-md transition"
          >
            <div className={`w-12 h-12 ${stat.color} rounded-lg flex items-center justify-center mb-4`}>
              <stat.icon className="w-6 h-6 text-white" />
            </div>
            <p className="text-3xl font-bold text-gray-900 mb-1">{stat.value}</p>
            <p className="text-gray-600">{stat.label}</p>
          </div>
        ))}
      </section>

      <section className="bg-white rounded-2xl shadow-sm p-8 md:p-12">
        <h2 className="text-3xl font-bold text-gray-900 mb-8 text-center">
          Por que escolher a EduPlatform?
        </h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {features.map((feature, index) => (
            <div key={index} className="text-center">
              <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <feature.icon className="w-8 h-8 text-blue-600" />
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">{feature.title}</h3>
              <p className="text-gray-600">{feature.description}</p>
            </div>
          ))}
        </div>
      </section>

      <section className="bg-gradient-to-r from-blue-600 to-blue-700 rounded-2xl shadow-lg p-8 md:p-12 text-center text-white">
        <h2 className="text-3xl font-bold mb-4">
          Pronto para começar sua jornada?
        </h2>
        <p className="text-xl mb-8 opacity-90">
          Junte-se a milhares de estudantes que já estão transformando suas carreiras.
        </p>
        <button
          onClick={onNavigateToCourses}
          className="px-8 py-4 bg-white text-blue-600 text-lg font-semibold rounded-xl hover:bg-gray-50 transition shadow-lg"
        >
          Começar Agora
        </button>
      </section>
    </div>
  );
};

const Clock = ({ className }: { className?: string }) => (
  <svg className={className} fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
  </svg>
);
